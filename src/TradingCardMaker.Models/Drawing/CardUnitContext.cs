namespace TradingCardMaker.Models.Drawing;

/// <summary>
/// Represents the current context of a card
/// </summary>
/// <param name="Root">The root sizes of the card</param>
/// <param name="Parent">The most recent parent of the size</param>
public record struct CardUnitContext(
    CardUnitContextData Root,
    CardUnitContextData? Parent)
{
    /// <summary>
    /// Optional width of the current context (null if no width is available)
    /// </summary>
    public readonly int? OptionalWidth => Parent?.Width ?? Root.Width;

    /// <summary>
    /// The width of the current context (exception if no width is available)
    /// </summary>
    public readonly int Width => OptionalWidth ?? throw new NullReferenceException("Width in this context is missing");

    /// <summary>
    /// Optional height of the current context (null if no height is available)
    /// </summary>
    public readonly int? OptionalHeight => Parent?.Height ?? Root.Height;

    /// <summary>
    /// The height in the current context (exception if no height is available)
    /// </summary>
    public readonly int Height => OptionalHeight ?? throw new NullReferenceException("Height in this context is missing");

    /// <summary>
    /// The optional font size of the current context (null if no font size is available)
    /// </summary>
    public readonly int? OptionalFontSize => Parent?.FontSize ?? Root.FontSize;

    /// <summary>
    /// The font size in the current context (exception if no height is available)
    /// </summary>
    public readonly int FontSize => OptionalFontSize ?? throw new NullReferenceException("Font size in this context is missing");
}