using Jint.Runtime.Modules;

namespace TradingCardMaker.Drawing.Utilities;

internal interface IUtility
{
    static abstract void Register(ModuleBuilder builder);
}
