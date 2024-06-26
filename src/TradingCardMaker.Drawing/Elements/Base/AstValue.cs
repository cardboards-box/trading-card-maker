﻿namespace TradingCardMaker.Drawing.Elements.Base;

using Templating.Ast;
using Templating.Scripting;

/// <summary>
/// Represents an attribute value that can be bound
/// </summary>
/// <typeparam name="T">The target end value</typeparam>
public class AstValue<T>
{
    /// <summary>
    /// The value of the attribute
    /// </summary>
    public T? Value { get; set; } = default;

    /// <summary>
    /// The context that bound this value
    /// </summary>
    public AstAttribute? Context { get; set; }

    /// <summary>
    /// The bind expression
    /// </summary>
    public ExpressionEvaluator? Bind { get; set; }
}
