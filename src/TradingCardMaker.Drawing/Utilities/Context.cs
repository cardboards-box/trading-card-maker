namespace TradingCardMaker.Drawing.Utilities;

using Loading;

public class Context(
    LoadedCardSet _context) : IUtility
{
    public object? get(string name)
    {
        return _context.Variables
            .TryGetValue(name, out var value)
            ? value : null;
    }
}
