namespace TradingCardMaker.Models;

using Drawing;

/// <summary>
/// Represents a single element displayed on a card.
/// </summary>
[JsonConverter(typeof(InterfaceParser<CardElement>))]
[Interface(typeof(ICardElement), nameof(Type), nameof(Config))]
public class CardElement
{
    /// <summary>
    /// The type of element to display
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// The size of the font within the element
    /// </summary>
    /// <remarks>
    /// Leave as null to infer from the parent.
    /// Json value can be a string (in CSS unit format) or a <see cref="CardUnit"/> instance.
    /// </remarks>
    [JsonPropertyName("fontSize")]
    public CardUnit? FontSize { get; set; }

    /// <summary>
    /// The bounds to render this element within
    /// </summary>
    /// <remarks>
    /// Leave as null to infer from the parent.
    /// Json value can be a string (in CSV CSS unit format) or a <see cref="CardRectangle"/> instance.
    /// </remarks>
    [JsonPropertyName("bounds")]
    public CardRectangle? Bounds { get; set; }

    /// <summary>
    /// Whether or not this element should be displayed
    /// </summary>
    [JsonPropertyName("enabled")]
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// The configuration settings unique to this element
    /// </summary>
    [JsonPropertyName("config")]
    public ICardElement? Config { get; set; }
}
