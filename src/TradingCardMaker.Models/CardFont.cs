namespace TradingCardMaker.Models;

using Core.IO;

/// <summary>
/// A font family used by the card
/// </summary>
public class CardFont
{
    /// <summary>
    /// The name of the font
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    /// <summary>
    /// The source URL of the font
    /// </summary>
    [JsonPropertyName("source")]
    public required IOPath Source { get; set; }
}