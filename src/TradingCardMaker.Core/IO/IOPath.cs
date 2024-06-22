namespace TradingCardMaker.Core.IO;

/// <summary>
/// A utility class for working with file and URI paths
/// </summary>
/// <param name="Value">The path to attach to</param>
public record struct IOPath(string Value)
{
    private string? _safePath;
    private IOPathType? _type;

    /// <summary>
    /// The character used to separate path segments in Windows
    /// </summary>
    public const char WINDOWS_PATH_SEPARATOR = '\\';

    /// <summary>
    /// The character used to separate path segments in Unix
    /// </summary>
    public const char UNIX_PATH_SEPARATOR = '/';

    /// <summary>
    /// A map between known URI schemes and IO path types
    /// </summary>
    public static Dictionary<string, IOPathType> PathTypeMap { get; } = new()
    {
        { Uri.UriSchemeHttp, IOPathType.REMOTE_HTTP },
        { Uri.UriSchemeHttps, IOPathType.REMOTE_HTTP },
        { Uri.UriSchemeFtp, IOPathType.REMOTE_FTP },
        { Uri.UriSchemeFtps, IOPathType.REMOTE_FTP },
        { Uri.UriSchemeFile, IOPathType.LOCAL_ABSOLUTE }
    };

    /// <summary>
    /// The type of path
    /// </summary>
    public IOPathType Type => _type ??= DetermineType(Value);

    /// <summary>
    /// The path in a format that is safe for the current operating system
    /// </summary>
    public string OSSafe => _safePath ??= CurrentOsSafe(Value, Type);

    /// <summary>
    /// Whether or not the file exists at the given path
    /// </summary>
    /// <remarks>Remote paths will always return false</remarks>
    public readonly bool Exists => FileExists(this);

    /// <summary>
    /// Get the absolute path of the current path
    /// </summary>
    /// <param name="relativeTo">The optional root directory to start relative paths in</param>
    /// <returns>The absolute path safe for the current OS</returns>
    public IOPath GetAbsolute(string? relativeTo = null)
    {
        if (!Type.HasFlag(IOPathType.Local)) return this;

        var path = OSSafe;
        if (!string.IsNullOrWhiteSpace(relativeTo) &&
            Type.HasFlag(IOPathType.Relative))
            path = Path.Combine(relativeTo, path);

        return new IOPath(Path.GetFullPath(path));
    }

    /// <summary>
    /// Gets the current value of the path
    /// </summary>
    /// <returns>The value of the path</returns>
    public readonly override string ToString() => Value;

    /// <summary>
    /// Converts between an IOPath and a string
    /// </summary>
    /// <param name="path">The IOPath</param>
    public static implicit operator string(IOPath path) => path.Value;

    /// <summary>
    /// Converts between a string and an IOPath
    /// </summary>
    /// <param name="path">The string</param>
    public static implicit operator IOPath(string path) => new(path);

    /// <summary>
    /// Checks whether the file exists at the given path
    /// </summary>
    /// <param name="path">The path to check</param>
    /// <returns>Whether or not the file exists</returns>
    /// <remarks>Remote paths will always return false</remarks>
    public static bool FileExists(IOPath path)
    {
        return path.Type.HasFlag(IOPathType.Local)
            && File.Exists(path.OSSafe);
    }

    /// <summary>
    /// Gets the path in the format the current operating system expects
    /// </summary>
    /// <param name="path">The path to convert</param>
    /// <param name="type">The type of path we're dealing with</param>
    /// <returns>The operating system safe path</returns>
    public static string CurrentOsSafe(string path, IOPathType type)
    {
        //Remote paths are assumed to be correct already
        if (type == IOPathType.Unknown ||
            !type.HasFlag(IOPathType.Local)) return path;

        //Local paths need to be converted to the correct value for the current OS
        var parts = path.Split(WINDOWS_PATH_SEPARATOR, UNIX_PATH_SEPARATOR);
        return Path.Combine(parts);
    }

    /// <summary>
    /// Determines the type of path based on the path string
    /// </summary>
    /// <param name="path">The path string</param>
    /// <returns>The type of path</returns>
    public static IOPathType DetermineType(string? path)
    {
        if (string.IsNullOrWhiteSpace(path)) return IOPathType.Unknown;

        if (Uri.TryCreate(path, UriKind.Absolute, out var uri))
            return PathTypeMap.TryGetValue(uri.Scheme, out var type) ? type : IOPathType.Unknown;

        if (path.StartsWith("./") ||
            path.StartsWith("../") ||
            path.StartsWith("..\\") ||
            path.StartsWith(".\\"))
            return IOPathType.LOCAL_RELATIVE;

        if (path.StartsWith("~/") ||
            path.StartsWith("~\\"))
            return IOPathType.LOCAL_RELATIVE_TILDE;

        try
        {
            //Last ditch attempt
            return File.Exists(path)
                ? IOPathType.LOCAL_RELATIVE
                : IOPathType.Unknown;
        }
        catch
        {
            return IOPathType.Unknown;
        }
    }
}
