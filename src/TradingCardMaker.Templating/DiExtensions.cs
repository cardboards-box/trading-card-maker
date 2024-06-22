namespace TradingCardMaker.Templating;

using Ast;

/// <summary>
///  The dependency injection extensions for the templating
/// </summary>
public static class DiExtensions
{
    /// <summary>
    /// Add the templating services to the given service collection
    /// </summary>
    /// <param name="services">The service collection to add templating to</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddTemplating(this IServiceCollection services)
    {
        return services
            .AddTransient<IAstParserService, AstParserService>();
    }
}
