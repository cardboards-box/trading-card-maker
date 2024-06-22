namespace TradingCardMaker.Models.Drawing;

/// <summary>
/// Represents a calculated rectangle on the card
/// </summary>
/// <param name="X">The start position of the rectangle on the X axis</param>
/// <param name="Y">The start position of the rectangle on the Y axis</param>
/// <param name="Width">The width of the rectangle</param>
/// <param name="Height">The height of the rectangle</param>
/// <param name="Context">The context this rectangle was calculated in</param>
public record struct CardRectanglePixel(
    int X,
    int Y,
    int Width,
    int Height,
    CardUnitContext Context);