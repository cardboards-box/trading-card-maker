namespace TradingCardMaker.Models;

using Core.CssUnit;

/// <summary>
/// Represents all of the designs available for a trading card
/// </summary>
public class CardSet
{
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
    public required CssUnit Width { get; set; }

    /// <summary>
    /// The height of the card (cannot be relative or percentage)
    /// </summary>
    [JsonPropertyName("height")]
    public required CssUnit Height { get; set; }

    /// <summary>
    /// The default font size for the card (cannot be relative or percentage)
    /// </summary>
    [JsonPropertyName("fontSize")]
    public required CssUnit FontSize { get; set; }

    /// <summary>
    /// The resources available to the card
    /// </summary>
    [JsonPropertyName("resources")]
    public CardResources Resources { get; set; } = new();

    /// <summary>
    /// The path to the template representing the back face of the card
    /// </summary>
    [JsonPropertyName("back")]
    public required string Back { get; set; }

    /// <summary>
    /// The IDs and paths to the templates representing the various variants of the font face of the card
    /// </summary>
    [JsonPropertyName("variants")]
    public Dictionary<string, string> Variants { get; set; } = [];

    /// <summary>
    /// Gets the root size context for the card set
    /// </summary>
    /// <returns>The size context for the card's bounds</returns>
    public SizeContext GetContext()
    {
        var width = Width.Pixels();
        var height = Height.Pixels();
        var fontSize = FontSize.Pixels();
        return SizeContext.ForRoot(width, height, fontSize);
    }
}
