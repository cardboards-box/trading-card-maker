namespace TradingCardMaker.Drawing.Utilities;

using Core.SizeUnit;
using Loading;

public class Drawing(
    LoadedCardSet _context) : IUtility
{
    public double UnitContext(string value, SizeContext? context, bool? isWidth)
    {
        context ??= _context.Original.GetContext();
        return SizeUnit.Parse(value).Pixels(context, isWidth);
    }

    public double unit(string value) => UnitContext(value, null, null);

    public double right(string value)
    {
        var ctx = _context.Original.GetContext();
        var size = UnitContext(value, ctx, true);
        return ctx.Root.Width - size;
    }

    public double left(string value) => UnitContext(value, null, true);

    public double top(string value) => UnitContext(value, null, false);

    public double bottom(string value)
    {
        var ctx = _context.Original.GetContext();
        var size = UnitContext(value, ctx, false);
        return ctx.Root.Height - size;
    }
}
