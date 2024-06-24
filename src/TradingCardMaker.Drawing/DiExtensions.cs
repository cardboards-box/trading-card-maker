namespace TradingCardMaker.Drawing;

using Loading;

/// <summary></summary>
public static class DiExtensions
{
    /// <summary>
    /// Register the drawing services with the service collection
    /// </summary>
    /// <param name="services">The service collection to register to</param>
    /// <returns>The service collection for method chaining</returns>
    public static IServiceCollection AddDrawing(this IServiceCollection services)
    {
        return services
            .AddTransient<ICardLoaderService, CardLoaderService>()
            .AddTransient<IFaceLoaderService, FaceLoaderService>();
    }
}
