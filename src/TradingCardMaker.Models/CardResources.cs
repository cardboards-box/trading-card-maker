namespace TradingCardMaker.Models;

/// <summary>
/// Represents all of the resources used by the card
/// </summary>
public class CardResources
{
    /// <summary>
    /// All of the custom font-families used by the card
    /// </summary>
    [JsonPropertyName("fonts")]
    public Dictionary<string, CardFont> Fonts { get; set; } = [];
}