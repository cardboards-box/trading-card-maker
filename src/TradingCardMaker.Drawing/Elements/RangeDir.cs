﻿namespace TradingCardMaker.Drawing.Elements;

/// <summary>
/// Represents a for directive
/// </summary>
[AstElement("range")]
public class RangeDir : DirectiveElement
{
    /// <summary>
    /// The start of value
    /// </summary>
    [AstAttribute("start")]
    public AstValue<double> Start { get; set; } = new();

    /// <summary>
    /// The end value of the loop
    /// </summary>
    [AstAttribute("end")]
    public AstValue<double> End { get; set; } = new();

    /// <summary>
    /// The step to increment each iteration by
    /// </summary>
    [AstAttribute("step")]
    public AstValue<double> Step { get; set; } = new();

    /// <summary>
    /// What to name the value in the children template contexts
    /// </summary>
    [AstAttribute("let")]
    public AstValue<double> Let { get; set; } = new();
}
