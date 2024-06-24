using Jint;
using Jint.Native;

namespace TradingCardMaker.Drawing.Rendering;

using Templating.Ast;

/// <summary>
/// Represents the rendering scope
/// </summary>
public class RenderScope
{
    /// <summary>
    /// The AST element that owns this scope
    /// </summary>
    public AstElement? AstElement { get; set; }

    /// <summary>
    /// The render element that owns this scope
    /// </summary>
    public IElement? Element { get; set; }

    /// <summary>
    /// All the variables in this scope
    /// </summary>
    public Dictionary<string, object> Variables { get; } = [];

    /// <summary>
    /// Sets the variables for the scope from the given value
    /// </summary>
    /// <param name="value">The value to attach by</param>
    public void Set(JsValue? value)
    {
        if (value is null ||
            value.IsUndefined() || 
            value.IsNull()) return;


        if (value.IsArray())
        {
            foreach (var item in value.AsArray())
                Set(item);
            return;
        }

        if (!value.IsObject()) return;

        var dic = value.AsObject().GetOwnProperties();
        foreach (var prop in dic)
        {
            var key = prop.Key.ToString();
            if (Variables.TryAdd(key, prop.Value))
                Variables[key] = prop.Value;
        }
    }

    /// <summary>
    /// Sets a variable in the current scope
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Set(string key, object value)
    {
        if (!Variables.TryAdd(key, value))
            Variables[key] = value;
    }
}
