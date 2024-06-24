using System.IO.Compression;

namespace TradingCardMaker.Drawing.Loading;

using Core.IO;
using Models;
using Templating.Ast;

/// <summary>
/// A service for loading cards
/// </summary>
public interface ICardLoaderService
{
    /// <summary>
    /// Loads the card set from the given path
    /// </summary>
    /// <param name="path">The path to load</param>
    /// <param name="config">The config for the AST parser</param>
    /// <returns>The loaded card set</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file could not be found</exception>
    /// <exception cref="InvalidOperationException">Thrown if the loaded card is null</exception>
    /// <exception cref="InvalidOperationException">Thrown if the file does not exist after downloading</exception>
    /// <exception cref="InvalidOperationException">Thrown if the zip file contains another zip file</exception>
    /// <exception cref="InvalidOperationException">Thrown if a module with the same name is already loaded</exception>
    Task<LoadedCardSet> Load(IOPath path, AstConfig? config = null);
}

internal class CardLoaderService(
    IFileResolverService _resolver,
    IFaceLoaderService _faces,
    ILogger<CardLoaderService> _logger) : ICardLoaderService
{
    /// <summary>
    /// Loads the card set from the given path
    /// </summary>
    /// <param name="path">The path to load</param>
    /// <param name="config">The config for the AST parser</param>
    /// <returns>The loaded card set</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file could not be found</exception>
    /// <exception cref="InvalidOperationException">Thrown if the loaded card is null</exception>
    /// <exception cref="InvalidOperationException">Thrown if the file does not exist after downloading</exception>
    /// <exception cref="InvalidOperationException">Thrown if the zip file contains another zip file</exception>
    /// <exception cref="InvalidOperationException">Thrown if a module with the same name is already loaded</exception>
    public Task<LoadedCardSet> Load(IOPath path, AstConfig? config = null)
    {
        config ??= AstConfig.Default;
        return path.Local
            ? LoadLocal(path, config)
            : LoadRemote(path, config);
    }

    /// <summary>
    /// Loads the card set from the given path
    /// </summary>
    /// <param name="path">The path to load from</param>
    /// <param name="config">The config for the AST parser</param>
    /// <returns>The loaded card set</returns>
    public async Task<LoadedCardSet> LoadLocal(IOPath path, AstConfig config)
    {
        //Determine the entry point of the file
        var (filePath, type) = DetermineEntryPoint(path.OSSafe);
        //If the file is a zip, load it
        if (type == EntryPointType.Zip)
            return await LoadZip(path.OSSafe, config);

        //Load the card set from the entry point file
        var card = await LoadCard(filePath);
        //Get absolute working directory
        var wrkDir = Path.GetDirectoryName(Path.GetFullPath(filePath))!;
        //Get entry point file name
        var entryName = Path.GetFileName(filePath);
        //Create the loaded card set
        var set = new LoadedCardSet
        {
            Original = card,
            WorkingDirectory = wrkDir,
            EntryPoint = entryName
        };

        //Load all of the global scripts
        foreach (var (name, script) in card.Resources.Scripts)
            await LoadScript(set, name, script);

        //Load the back face of the card set if specified
        if (card.Back is not null)
            set.BackFace = await _faces.Load(card.Back.Value, set, config);

        //Load all of the front faces of the card set
        foreach (var (name, face) in card.Variants)
        {
            if (face is null) continue;

            var loaded = await _faces.Load(face.Value, set, config);
            set.FrontFaces.Add(name, loaded);
        }
        //Return the loaded set
        return set;
    }

    /// <summary>
    /// Loads the script from the given path and caches it's prepared module in the script cache
    /// </summary>
    /// <param name="set">The card set to load it to</param>
    /// <param name="name">The name of the module</param>
    /// <param name="path">The file path to load</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException">Thrown if a module with the same name is already loaded</exception>
    public async Task LoadScript(LoadedCardSet set, string name, IOPath path)
    {
        //Validate the script has a unique name
        if (set.Scripts.ContainsKey(name))
            throw new InvalidOperationException($"Already loaded script with name: {name} >> {path.OSSafe}");

        //Prepare the script from the file
        var module = await _faces.PrepareFromFile(path, set);
        //Add the script to the cache
        set.Scripts.Add(name, module);
    }

    /// <summary>
    /// Loads a card set from a zip file
    /// </summary>
    /// <param name="path">The path the zip file is present in</param>
    /// <param name="config">The config for the AST parser</param>
    /// <returns>The loaded card set</returns>
    /// <exception cref="InvalidOperationException">Thrown if the zip file contains another zip file</exception>
    public async Task<LoadedCardSet> LoadZip(string path, AstConfig config)
    {
        //Create a random directory to store the extracted files
        var dir = IOPathHelper.RandomDirectory();
        try
        {
            //Extract the zip file
            ZipFile.ExtractToDirectory(path, dir);
            //Get the entry point file path
            var (file, type) = DetermineEntryPoint(dir);
            //If it's a zip file, throw an exception because zip-ception
            if (type == EntryPointType.Zip)
                throw new InvalidOperationException("Failed to extract zip file - Why the zip-ception?");

            //Load the card set from the entry point file
            var card = await LoadLocal(file, config);
            //Add the directory to the cleanup list
            card.Cleanup.Add(dir);
            return card;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load card set - Zip file");
            //Delete the directory if it exists - Clean up after ourselves, it's only polite.
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            throw;
        }
    }

    /// <summary>
    /// Loads a card set from a remote source
    /// </summary>
    /// <param name="path">The remote source</param>
    /// <param name="config">The config for the AST parser</param>
    /// <returns>The loaded card set</returns>
    /// <exception cref="InvalidOperationException">Thrown if the file does not exist after downloading</exception>
    public async Task<LoadedCardSet> LoadRemote(IOPath path, AstConfig config)
    {
        //Load the file from the end point
        var (stream, file, type) = await _resolver.Fetch(path);
        //Get the extension from the mime-type
        var ext = IOPathHelper.DetermineExtension(type);
        //Get the file name or generate a random one
        var fileName = string.IsNullOrEmpty(file)
            ? $"{Path.GetRandomFileName()}.{ext}" : file;
        //Get a random directory to store the file in
        var dir = IOPathHelper.RandomDirectory();
        try
        {
            //Save the file to the directory
            var outputPath = Path.Combine(dir, fileName);
            using (var fileStream = File.Create(outputPath))
            {
                await stream.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
            //Shouldn't happen... but just in case
            if (!File.Exists(outputPath))
                throw new InvalidOperationException("Failed to save file - Invalid path?");
            //Load the card from the file
            var card = await LoadLocal(outputPath, config);
            //Add the directory to the clean up list
            card.Cleanup.Add(dir);
            return card;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load card set - Remote file");
            //Delete the directory if it exists - Clean up after ourselves, it's only polite.
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            throw;
        }
    }

    /// <summary>
    /// Load the card information from a file
    /// </summary>
    /// <param name="path">The to load from</param>
    /// <returns>The card information</returns>
    /// <exception cref="InvalidOperationException">Thrown if the loaded card is null</exception>
    public static async Task<CardSet> LoadCard(string path)
    {
        CardSet? card;
        using (var stream = File.OpenRead(path))
            card = await JsonSerializer.DeserializeAsync<CardSet>(stream);
        return card
            ?? throw new InvalidOperationException("Failed to deserialize card set - Invalid JSON?");
    }

    /// <summary>
    /// Checks to see if the path is a file, directory, or zip file
    /// </summary>
    /// <param name="path">The path to check</param>
    /// <returns>The file's path and the type of resolver</returns>
    /// <exception cref="FileNotFoundException">Thrown if the file could not be found</exception>
    public (string filePath, EntryPointType type) DetermineEntryPoint(string path)
    {
        //Check if the file exists
        if (File.Exists(path))
        {
            //Get the extension of the file
            var ext = Path.GetExtension(path).ToLower().Trim('.');
            //Determine type of file based on extension            
            return ext == "zip"
                ? (path, EntryPointType.Zip)
                : (path, EntryPointType.File);
        }

        //Get all of the valid file names and extensions
        string[] fileNames = ["index", "main", "card"];
        string[] extensions = ["json", "cards"];
        //Get all of the possible file names
        var files = fileNames.SelectMany(name => extensions.Select(ext => $"{name}.{ext}"));
        //Find any matching file
        foreach (var file in files)
        {
            //Get the full path of the file
            var fullPath = Path.Combine(path, file);
            //Check if it exists
            if (File.Exists(fullPath))
                return (fullPath, EntryPointType.Directory);
        }
        //Couldn't find any matching files
        throw new FileNotFoundException($"No entry point found. Searched: {string.Join(", ", files)}", path);
    }

    /// <summary>
    /// Indicates the type of entry point for the cards
    /// </summary>
    public enum EntryPointType
    {
        /// <summary>
        /// Entry point is a JSON file 
        /// </summary>
        File = 1,
        /// <summary>
        /// Entry point is a directory
        /// </summary>
        Directory = 2,
        /// <summary>
        /// Entry point has a .zip extension
        /// </summary>
        Zip = 3,
    }
}
