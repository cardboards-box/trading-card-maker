namespace TradingCardMaker.Models;

using Core.SizeUnit;
using Core.IO;

/// <summary>
/// Represents all of the designs available for a trading card
/// </summary>
public class CardSet
{
    private SizeContext? _context;

    /// <summary>
    /// The name of the card set
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The description of the card set
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    /// <summary>
    /// The width of the card (cannot be relative or percentage)
    /// </summary>
    [JsonPropertyName("width")]
    public required SizeUnit Width { get; set; }

    /// <summary>
    /// The height of the card (cannot be relative or percentage)
    /// </summary>
    [JsonPropertyName("height")]
    public required SizeUnit Height { get; set; }

    /// <summary>
    /// The default font size for the card (cannot be relative or percentage)
    /// </summary>
    [JsonPropertyName("fontSize")]
    public required SizeUnit FontSize { get; set; }

    /// <summary>
    /// The resources available to the card
    /// </summary>
    [JsonPropertyName("resources")]
    public CardResources Resources { get; set; } = new();

    /// <summary>
    /// The path to the template representing the back face of the card
    /// </summary>
    [JsonPropertyName("back")]
    public IOPath? Back { get; set; }

    /// <summary>
    /// The IDs and paths to the templates representing the various variants of the font face of the card
    /// </summary>
    [JsonPropertyName("variants")]
    public Dictionary<string, IOPath?> Variants { get; set; } = [];

    /// <summary>
    /// Animation settings for the card
    /// </summary>
    /// <remarks>null means not to animate</remarks>
    [JsonPropertyName("animate")]
    public CardAnimate? Animate { get; set; }

    /// <summary>
    /// Gets the root size context for the card set
    /// </summary>
    /// <returns>The size context for the card's bounds</returns>
    public SizeContext GetContext()
    {
        if (_context is not null) return _context;

        var width = Width.Pixels();
        var height = Height.Pixels();
        var fontSize = FontSize.Pixels();
        return _context = SizeContext.ForRoot(width, height, fontSize);
    }
}
