namespace TradingCardMaker.Models;

/// <summary>
/// Represents a render pipeline element for the card
/// </summary>
public interface ICardElement
{
    /// <summary>
    /// The type of element being processed
    /// </summary>
    string Type { get; set; }
}
