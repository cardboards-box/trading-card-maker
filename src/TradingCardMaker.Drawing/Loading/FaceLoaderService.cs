using Module = Jint.Prepared<Esprima.Ast.Module>;

namespace TradingCardMaker.Drawing.Loading;

using Core.IO;
using Templating.Ast;
using Templating.Scripting;
using Utilities;

/// <summary>
/// Service for loading card faces into memory
/// </summary>
public interface IFaceLoaderService
{
    /// <summary>
    /// Loads all of the script and elements from the template
    /// </summary>
    /// <param name="path">The path to the face template</param>
    /// <param name="set">The card set to load the card into</param>
    /// <param name="config">The configuration for the AST parser</param>
    /// <returns>The loaded card</returns>
    Task<LoadedCard> Load(IOPath path, LoadedCardSet set, AstConfig config);

    /// <summary>
    /// Prepares the script for execution in the script runner
    /// </summary>
    /// <param name="path"></param>
    /// <param name="set"></param>
    /// <returns></returns>
    Task<Module> PrepareFromFile(IOPath path, LoadedCardSet set);
}

internal class FaceLoaderService(
    IFileResolverService _resolver,
    IAstParserService _parser,
    IElementReflectionService _elements) : IFaceLoaderService
{
    public async Task<LoadedCard> Load(IOPath path, LoadedCardSet set, AstConfig config)
    {
        var actualPath = path.GetAbsolute(set.WorkingDirectory);
        var (stream, _, _) = await _resolver.Fetch(actualPath);
        var elements = _parser.ParseStream(stream, config);

        var card = new LoadedCard();

        foreach (var element in elements)
        {
            card.Elements.Add(element);

            var tag = element.Tag.ToLower().Trim();
            if (tag == "template")
            {
                HandleTemplate(card, element);
                continue;
            }

            if (tag != "script")
                throw new InvalidOperationException(
                    "Only script and template tags allowed in face root: " +
                    element.ExceptionString());

            await HandleScript(element, card, set);
        }

        card.Runner = GenerateRunner(card, set);
        return card;
    }

    public static void AddStandardContext(ScriptRunner runner)
    {
        runner.AddModule("system", t => t
            .ExportType<Drawing>()
            .ExportType<Context>());
    }

    public static ScriptRunner? GenerateRunner(LoadedCard card, LoadedCardSet set)
    {
        if (card.Setup is null) return null;

        var runner = new ScriptRunner();

        AddStandardContext(runner);
        runner.AddModule("face-script", card.Setup.Value);

        foreach (var (name, script) in set.Scripts)
            runner.AddModule(name, script);

        foreach (var (name, script) in card.Scripts)
            runner.AddModule(name, script);

        return runner.SetScript(@"
import FaceScript from 'face-script';
export function main(args) { 
    return FaceScript(args); 
}");
    }

    public async Task HandleScript(AstElement element, LoadedCard card, LoadedCardSet set)
    {
        var isSetup = IsSetup(element);
        var isSrc = IsSrc(element, out var src);

        if (isSetup && card.Setup is not null)
            throw new InvalidOperationException(
                "Cannot have more than 1 setup script: "
                + element.ExceptionString());

        Module? module;
        if (isSrc)
            module = await PrepareFromFile(src!.Value, set);
        else if (!string.IsNullOrWhiteSpace(element.Value))
            module = ScriptRunner.Prepare(element.Value);
        else
            throw new InvalidOperationException(
                "Could not determine script to prepare: " +
                element.ExceptionString());

        if (isSetup)
        {
            card.Setup = module;
            return;
        }

        var name = GetName(element);
        if (card.Scripts.ContainsKey(name) ||
            set.Scripts.ContainsKey(name))
            throw new InvalidOperationException(
                "Script already exists with the same name: " +
                element.ExceptionString());

        card.Scripts.Add(name, module!.Value);
    }

    public async Task<Module> PrepareFromFile(IOPath path, LoadedCardSet set)
    {
        //Get the actual path relative to the working directory 
        var actualPath = path.GetAbsolute(set.WorkingDirectory);
        //Resolve the script from the source
        var (stream, _, _) = await _resolver.Fetch(actualPath);
        //Read the script to the end
        using var reader = new StreamReader(stream);
        var script = await reader.ReadToEndAsync();
        await stream.DisposeAsync();
        //Prepare the script and return
        return ScriptRunner.Prepare(script);
    }

    public static string GetName(AstElement element)
    {
        var name = element.Attributes
            .FirstOrDefault(t => t.Name.ToLower().Trim() == "name")?
            .Value;
        if (!string.IsNullOrWhiteSpace(name)) return name;

        throw new InvalidOperationException(
            "Non-setup script does not have a name attribute: "
            + element.ExceptionString());
    }

    public static bool IsSetup(AstElement element)
    {
        var attribute = element.Attributes
            .FirstOrDefault(t => t.Name.ToLower().Trim() == "setup");

        if (attribute is null) return false;
        if (attribute.Type == AstAttributeType.BooleanTrue) return true;

        var value = attribute.Value?.ToLower().Trim();
        return value == "true";
    }

    public static bool IsSrc(AstElement element, out IOPath? src)
    {
        var attribute = element.Attributes
            .FirstOrDefault(t => t.Name.ToLower().Trim() == "src");
        src = null;

        if (attribute is null) return false;
        if (string.IsNullOrEmpty(attribute.Value)) return false;

        src = attribute.Value;
        return true;
    }

    public void HandleTemplate(LoadedCard card, AstElement element)
    {
        if (card.Template.Count != 0)
            throw new InvalidOperationException(
                "Template has already been loaded for card: " +
                element.ExceptionString());

        foreach (var child in element.Children)
            card.Elements.Add(child);

        foreach (var template in _elements.BindTemplates(element.Children, true))
            card.Template.Add(template);
    }
}
