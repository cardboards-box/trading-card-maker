namespace TradingCardMaker.Models;

/// <summary>
/// The dependency injection extensions for the models
/// </summary>
public static class DiExtensions
{
    /// <summary>
    /// Add the model services to the given service collection
    /// </summary>
    /// <param name="services">The service collection to add models to</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddModels(this IServiceCollection services)
    {
        return services;
    }
}
