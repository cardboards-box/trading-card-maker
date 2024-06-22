namespace TradingCardMaker.Models.Drawing;

using Helpers;

/// <summary>
/// Represents a rectangle on the card
/// </summary>
/// <param name="X">The start position of the rectangle on the X axis</param>
/// <param name="Y">The start position of the rectangle on the Y axis</param>
/// <param name="Width">The width of the rectangle</param>
/// <param name="Height">The height of the rectangle</param>
public record struct CardRectangle(
    [property: JsonPropertyName("x")] CardUnit X,
    [property: JsonPropertyName("y")] CardUnit Y,
    [property: JsonPropertyName("width")] CardUnit Width,
    [property: JsonPropertyName("height")] CardUnit Height)
{
    /// <summary>
    /// Fill the entire card
    /// </summary>
    public static CardRectangle Fill { get; } = new(
        CardUnit.Zero, CardUnit.Zero,
        CardUnit.Fill, CardUnit.Fill);

    /// <summary>
    /// Calculates the pixel values of the rectangle
    /// </summary>
    /// <param name="context">The context of the unit</param>
    /// <returns>The calculated rectangle</returns>
    public readonly CardRectanglePixel Bind(CardUnitContext context)
    {
        var x = X.Pixels(context, true);
        var y = Y.Pixels(context, false);
        var w = Width.Pixels(context, true);
        var h = Height.Pixels(context, false);
        return new(x, y, w, h, context);
    }

    /// <summary>
    /// Converts the rectangle to a string
    /// </summary>
    /// <returns>The string representation of the rectangle</returns>
    public readonly string Serialize() => CssUnitHelper.SerializeRectangle(this);

    /// <summary>
    /// Converts the rectangle to a string
    /// </summary>
    /// <returns>The string representation of the rectangle</returns>
    public override readonly string ToString() => Serialize();

    /// <summary>
    /// Converts the string to a rectangle
    /// </summary>
    /// <param name="input">The string version of the rectangle</param>
    /// <returns>The parsed rectangle</returns>
    public static CardRectangle Parse(string input) => CssUnitHelper.ParseRectangle(input);

    /// <summary>
    /// Converts the string to a rectangle
    /// </summary>
    /// <param name="value">The string version of the rectangle</param>
    public static implicit operator CardRectangle(string value) => Parse(value);

    /// <summary>
    /// Converts the rectangle to a string
    /// </summary>
    /// <param name="value">The rectangle to serialize to a string</param>
    public static implicit operator string(CardRectangle value) => value.Serialize();
}