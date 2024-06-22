namespace TradingCardMaker.Models.Drawing;

/// <summary>
/// Represents a context for card size
/// </summary>
/// <param name="Width">The width of the current context</param>
/// <param name="Height">The height of the current context</param>
/// <param name="FontSize">The font size in the current context</param>
public record struct CardUnitContextData(
    int? Width,
    int? Height,
    int? FontSize);
