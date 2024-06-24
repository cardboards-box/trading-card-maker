using Module = Jint.Prepared<Esprima.Ast.Module>;

namespace TradingCardMaker.Drawing.Loading;

using Templating.Ast;
using Templating.Scripting;

/// <summary>
/// Represents a card face loaded into memory
/// </summary>
public class LoadedCard
{
    /// <summary>
    /// The script to run to setup the template
    /// </summary>
    public Module? Setup { get; set; }

    /// <summary>
    /// The cached script runner 
    /// </summary>
    public ScriptRunner? Runner { get; set; }

    /// <summary>
    /// Any scripts to import into a runtime context
    /// </summary>
    public Dictionary<string, Module> Scripts { get; } = [];

    /// <summary>
    /// The template elements used to render the card
    /// </summary>
    public List<IElement> Template { get; } = [];

    /// <summary>
    /// All of the elements from the AST template
    /// </summary>
    public List<AstElement> Elements { get; set; } = [];
}
