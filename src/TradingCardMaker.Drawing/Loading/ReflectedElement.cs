namespace TradingCardMaker.Drawing.Loading;

using Templating.Ast;

/// <summary>
/// Represents the element binds to POCOs
/// </summary>
/// <param name="Type">The concrete class type</param>
/// <param name="Attribute">The attributes on the element</param>
/// <param name="ChildType">The type of children the element is expecting</param>
/// <param name="ChildProperty">The property pointing to the child value</param>
/// <param name="Constructors">The constructor parameters (for dependency injection)</param>
/// <param name="Props">All of the properties on the class with the <see cref="AstAttributeAttribute"/> attribute</param>
public record class ReflectedElement(
    Type Type,
    AstElementAttribute[] Attribute,
    AstElementType ChildType,
    PropertyInfo? ChildProperty,
    ParameterInfo[] Constructors,
    ReflectedAttribute[] Props);

/// <summary>
/// Represents the attribute binds to POCO properties
/// </summary>
/// <param name="Type">The property info</param>
/// <param name="Attributes">The attributes on the property</param>
/// <param name="IsBindable">Whether or not the value is a <see cref="AstValue{T}"/></param>
/// <param name="BindType">The generic type of the <see cref="AstValue{T}"/> if <paramref name="IsBindable"/> is true</param>
public record class ReflectedAttribute(
    PropertyInfo Type,
    AstAttributeAttribute[] Attributes,
    bool IsBindable,
    Type? BindType);