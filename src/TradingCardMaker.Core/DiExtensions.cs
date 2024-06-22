using TradingCardMaker.Core.IO;

namespace TradingCardMaker.Core;

/// <summary></summary>
public static class DiExtensions
{
    /// <summary></summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services
            .AddJson()
            .AddCardboardHttp()
            .AddFileCache()
            .AddTransient<IFileResolverService, FileResolverService>();
    }
}
