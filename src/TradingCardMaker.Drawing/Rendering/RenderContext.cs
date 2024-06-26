namespace TradingCardMaker.Drawing.Rendering;

using Loading;
using Templating.Scripting;

/// <summary>
/// The context for a rendering pipeline
/// </summary>
public class RenderContext
{
    /// <summary>
    /// The card set the context is for
    /// </summary>
    public required LoadedCardSet CardSet { get; init; }

    /// <summary>
    /// The card the context is for
    /// </summary>
    public required LoadedCard Card { get; init; }

    /// <summary>
    /// All of the scopes for the context
    /// </summary>
    public List<RenderScope> ScopeStack { get; set; } = [];

    /// <summary>
    /// Adds a scope to the stack
    /// </summary>
    /// <param name="scope">The render scope to add</param>
    public void AddScope(RenderScope scope)
    {
        ScopeStack.Add(scope);
    }

    /// <summary>
    /// Remove the last scope from the stack
    /// </summary>
    public void RemoveLastScope()
    {
        if (ScopeStack.Count <= 1) return;

        ScopeStack.RemoveAt(ScopeStack.Count - 1);
    }

    /// <summary>
    /// Sets the scope of the expression evaluator
    /// </summary>
    /// <param name="expression"></param>
    public void SetScope(ExpressionEvaluator expression)
    {
        expression.SetContext(new Dictionary<string, object>
        {
            ["set"] = CardSet,
            ["card"] = Card,
            ["scope"] = ScopeStack,
            ["frame"] = CardSet.CurrentFrame,
        });

        foreach (var scope in ScopeStack)
            expression.SetContext(scope.Variables);
    }
}
