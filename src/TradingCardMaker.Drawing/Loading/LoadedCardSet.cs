using Module = Jint.Prepared<Esprima.Ast.Module>;

namespace TradingCardMaker.Drawing.Loading;

using Models;

/// <summary>
/// A <see cref="CardSet"/> that has been loaded onto the current machine
/// </summary>
public class LoadedCardSet
{
    /// <summary>
    /// The original card set
    /// </summary>
    public required CardSet Original { get; init; }

    /// <summary>
    /// The working directory 
    /// </summary>
    public required string WorkingDirectory { get; init; }

    /// <summary>
    /// The file name of the entry point
    /// </summary>
    public required string EntryPoint { get; init; }

    /// <summary>
    /// The loaded back face of the card
    /// </summary>
    public LoadedCard? BackFace { get; set; }

    /// <summary>
    /// The variants of the front faces of the card
    /// </summary>
    public Dictionary<string, LoadedCard> FrontFaces { get; } = [];

    /// <summary>
    /// All of the scripts that are referenced by the card set
    /// </summary>
    public Dictionary<string, Module> Scripts { get; } = [];

    /// <summary>
    /// All of the file paths that need to be cleaned up when the card is done
    /// </summary>
    public List<string> Cleanup { get; } = [];

    /// <summary>
    /// Any variables that can be used in the faces
    /// </summary>
    public Dictionary<string, object?> Variables { get; } = [];

    /// <summary>
    /// The current frame number
    /// </summary>
    public int CurrentFrame { get; set; } = 0;
}
