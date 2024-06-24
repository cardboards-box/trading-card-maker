namespace TradingCardMaker.Models;

using Core.IO;

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

    /// <summary>
    /// Scripts that are imported into the script context on every card
    /// </summary>
    /// <remarks>Key is the module name, value is the path to the script</remarks>
    [JsonPropertyName("scripts")]
    public Dictionary<string, IOPath> Scripts { get; set; } = [];
}