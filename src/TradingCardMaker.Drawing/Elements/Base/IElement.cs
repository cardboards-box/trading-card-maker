﻿namespace TradingCardMaker.Drawing.Elements.Base;

using Templating.Ast;

/// <summary>
/// Indicates that a class is a drawing abstract syntax tree element
/// </summary>
public interface IElement
{
    /// <summary>
    /// The <see cref="AstElement"/> that created this element
    /// </summary>
    /// <remarks>This can be used to find the original position of the element within AST</remarks>
    AstElement? Context { get; set; }
}