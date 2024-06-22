namespace TradingCardMaker.Templating.Ast;

/// <summary>
/// Represents an element in the custom XML AST for card templates
/// </summary>
public class AstElement
{
    /// <summary>
    /// The tag of the element
    /// </summary>
    public required string Tag { get; set; }

    /// <summary>
    /// Indicates the type of children on this element
    /// </summary>
    public required AstElementType Type { get; set; }

    /// <summary>
    /// Any attributes present on the element
    /// </summary>
    public AstAttribute[] Attributes { get; set; } = [];

    /// <summary>
    /// Any children element (only applicable if <see cref="Type"/> is <see cref="AstElementType.Children"/>)"/>
    /// </summary>
    public AstElement[] Children { get; set; } = [];

    /// <summary>
    /// The text content of the element (only applicable if <see cref="Type"/> is <see cref="AstElementType.Text"/>)"/>
    /// </summary>
    public string? Value { get; set; }
}
