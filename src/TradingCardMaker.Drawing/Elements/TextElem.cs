namespace TradingCardMaker.Drawing.Elements;

using Core.SizeUnit;

/// <summary>
/// Represents text to be drawn to the card
/// </summary>
[AstElement("text")]
public class TextElem : PositionalGDIElement
{
    /// <summary>
    /// The value of the text to draw to the card
    /// </summary>
    [AstAttribute("value")]
    public AstValue<string> Value { get; set; } = new();

    /// <summary>
    /// The font size
    /// </summary>
    [AstAttribute("font-size")]
    public AstValue<SizeUnit> FontSize { get; set; } = new();

    /// <summary>
    /// The color to fill with
    /// </summary>
    [AstAttribute("color")]
    public AstValue<string> Color { get; set; } = new();

    /// <summary>
    /// The color to fill with
    /// </summary>
    [AstAttribute("vertical-align")]
    public AstValue<string> VerticalAlign { get; set; } = new();
}
