namespace TradingCardMaker.Core.CssUnit;

/// <summary>
/// A utility class for working with regular expressions
/// </summary>
public static partial class RegexUtility
{
    /// <summary>
    /// Strips all non-alphanumeric characters from a string (excluding percentages, periods, and hyphens)
    /// </summary>
    /// <param name="input">The string to strip</param>
    /// <returns>The stripped string</returns>
    public static string CssUnitFilter(string input)
    {
        return CssUnitFilterRegex().Replace(input, "");
    }

    /// <summary>
    /// Gets the value and unit of a CSS unit
    /// </summary>
    /// <param name="input">The given CSS unit</param>
    /// <returns>The parsed value and unit</returns>
    public static (double value, string unit) CssUnitParse(string input)
    {
        var regex = CssUnitParserRegex();
        var match = regex.Match(input);
        if (!match.Success) throw CssUnitParserException.RegexFailed();
        if (match.Groups.Count <= 1) throw CssUnitParserException.NoMatches();

        var strValue = match.Groups[1].Value;
        if (string.IsNullOrEmpty(strValue)) throw CssUnitParserException.NoValue();

        if (!double.TryParse(strValue, out var value)) throw CssUnitParserException.InvalidValue();

        if (match.Groups.Count == 1) return (value, string.Empty);

        var unit = match.Groups[2].Value;
        return (value, unit);
    }

    [GeneratedRegex(@"[^a-zA-Z0-9\.%-]")]
    private static partial Regex CssUnitFilterRegex();

    [GeneratedRegex(@"(-?[0-9]{0,}\.?[0-9]{0,}?)([a-z%]{1,})?$")]
    private static partial Regex CssUnitParserRegex();
}

/// <summary>
/// Exception thrown when a CSS unit parser fails
/// </summary>
/// <param name="message">The error that occurred</param>
public class CssUnitParserException(string message) : Exception(message) 
{
    internal static CssUnitParserException RegexFailed() => new("Input string is not a valid CSS unit (Failed validation)");

    internal static CssUnitParserException NoMatches() => new("Input string is not a valid CSS unit (Failed validation - no matches)");

    internal static CssUnitParserException NoValue() => new("Input string is not a valid CSS unit (No value)");

    internal static CssUnitParserException InvalidValue() => new("Input string is not a valid CSS unit (Unit value was not a valid number)");
}