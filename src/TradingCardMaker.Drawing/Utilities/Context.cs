using Jint.Runtime.Modules;
using TradingCardMaker.Drawing.Loading;

namespace TradingCardMaker.Drawing.Utilities;

public class Context(
    LoadedCardSet _context) : IUtility
{
    public object? get(string name)
    {
        return _context.Variables
            .TryGetValue(name, out var value)
            ? value : null;
    }

    public static void Register(ModuleBuilder builder)
    {
        builder.ExportType<Context>();
    }
}
