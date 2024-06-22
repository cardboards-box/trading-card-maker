namespace TradingCardMaker.Templating.Ast;

/// <summary>
/// Some customizable options for AST parsing
/// </summary>
/// <param name="AttributeBind">The character that appears before script bind attributes</param>
/// <param name="AttributeSpreadStart">The character that indicates the start of a spread attribute</param>
/// <param name="AttributeSpreadEnd">The character that indicates the end of a spread attribute</param>
public record class AstConfig(
    char AttributeBind,
    char AttributeSpreadStart,
    char AttributeSpreadEnd)
{
    /// <summary>
    /// The default settings for AST parsing
    /// </summary>
    public static AstConfig Default { get; } = new(':', '{', '}');
}