namespace TradingCardMaker.Core.Helpers;

/// <summary>
/// Helper utility for css unit operations
/// </summary>
public static class CssUnitHelper
{
    private static CssUnit[]? _units;
    private const string ERROR_PERCENT = "Cannot use percentage measurement as the context of the size is not known";

    /// <summary>
    /// All of the available units of measurement
    /// </summary>
    /// <returns></returns>
    public static CssUnit[] Units()
    {
        return _units ??=
        [
            new CssUnit(CardUnitType.Centimeter, "cm"),
            new CssUnit(CardUnitType.Millimeter, "mm"),
            new CssUnit(CardUnitType.QuarterMillimeter, "q"),
            new CssUnit(CardUnitType.Inch, "in"),
            new CssUnit(CardUnitType.Pica, "pc"),
            new CssUnit(CardUnitType.Point, "pt"),
            new CssUnit(CardUnitType.Pixel, "px"),
            new CssUnit(CardUnitType.Percentage, "%"),
            new CssUnit(CardUnitType.Em, "em"),
            new CssUnit(CardUnitType.ViewHeight, "vh"),
            new CssUnit(CardUnitType.ViewWidth, "vw"),
            new CssUnit(CardUnitType.RelativePercentage, "rp")
        ];
    }

    /// <summary>
    /// Converts the given string to a <see cref="CardUnit"/>
    /// </summary>
    /// <param name="input">The unit of measurement</param>
    /// <returns>The <see cref="CardUnit"/> representing the unit of measurement</returns>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="input"/> is null or empty</exception>
    /// <exception cref="CssUnitParserException">Thrown if the <paramref name="input"/> is not a valid unit of measurement</exception>
    public static CardUnit ParseUnit(string input)
    {
        if (string.IsNullOrEmpty(input)) throw new ArgumentNullException(nameof(input));

        input = input.Trim().ToLower();

        if (input == "0") return CardUnit.Zero;

        input = RegexUtility.CssUnitFilter(input);
        if (string.IsNullOrEmpty(input)) return CardUnit.Zero;

        var (value, unit) = RegexUtility.CssUnitParse(input);
        if (string.IsNullOrEmpty(unit)) return new CardUnit(CardUnitType.Pixel, value);

        var unitMatch = Units().FirstOrDefault(u => u.Symbol == unit);
        if (unitMatch is null) return new CardUnit(CardUnitType.Pixel, value);

        return new CardUnit(unitMatch.Type, value);
    }

    /// <summary>
    /// Serializes the given <see cref="CardUnit"/> to a string
    /// </summary>
    /// <param name="size">The unit of measurement</param>
    /// <returns>The string representation of the size</returns>
    public static string SerializeUnit(CardUnit size)
    {
        var unit = Units().FirstOrDefault(u => u.Type == size.Type);
        if (unit is null) return $"{size.Value}px";

        return $"{size.Value}{unit.Symbol}";
    }

    /// <summary>
    /// Converts the <see cref="CardUnit"/> into the pixel equivalent
    /// </summary>
    /// <param name="size">The unit to convert</param>
    /// <param name="context">The context of the size request</param>
    /// <param name="isWidth">Whether or not the unit is for widths or heights (or null if the context isn't known)</param>
    /// <returns>The pixel equivalent of the given size</returns>
    /// <exception cref="NullReferenceException">Thrown if the context is missing a required value</exception>
    /// <exception cref="ArgumentException">Thrown if the size is a percentage and the context is not known</exception>
    public static int GetPixels(CardUnit size, SizeContext? context, bool? isWidth)
    {
        if (size.Value == 0) return 0;

        SizeContext NotNullContext() => context ?? throw new NullReferenceException(ERROR_PERCENT);

        var value = size.Type switch
        {
            CardUnitType.Pixel => size.Value,
            CardUnitType.Centimeter => CentimeterToPixel(size.Value),
            CardUnitType.Millimeter => MillimeterToPixel(size.Value),
            CardUnitType.QuarterMillimeter => QuarterMillimeterToPixel(size.Value),
            CardUnitType.Inch => InchToPixel(size.Value),
            CardUnitType.Pica => PicaToPixel(size.Value),
            CardUnitType.Point => PointToPixel(size.Value),
            CardUnitType.RelativePercentage => RelativePercentageToPixel(size.Value, NotNullContext().Width, NotNullContext().Height),
            CardUnitType.Em => EmToPixel(size.Value, NotNullContext().FontSize),
            CardUnitType.ViewHeight => PercentToPixel(size.Value, null, NotNullContext().Root.Height, false),
            CardUnitType.ViewWidth => PercentToPixel(size.Value, NotNullContext().Root.Width, null, true),
            CardUnitType.Percentage => PercentToPixel(size.Value, NotNullContext().Width, NotNullContext().Height, isWidth),
            _ => 0,
        };

        return (int)Math.Round(value);
    }

    /// <summary>
    /// Converts the given centimeters to pixels
    /// </summary>
    /// <param name="value">The number of centimeters</param>
    /// <returns>The equivalent number of pixels</returns>
    public static double CentimeterToPixel(double value) => value * 37.795275590551181102362204724409;

    /// <summary>
    /// Converts the given millimeters to pixels
    /// </summary>
    /// <param name="value">The number of millimeters</param>
    /// <returns>The equivalent number of pixels</returns>
    public static double MillimeterToPixel(double value) => CentimeterToPixel(value) / 10;

    /// <summary>
    /// Converts the given quarter millimeters to pixels
    /// </summary>
    /// <param name="value">The number of quarter millimeters</param>
    /// <returns>The equivalent number of pixels</returns>
    public static double QuarterMillimeterToPixel(double value) => MillimeterToPixel(value) / 4;

    /// <summary>
    /// Converts the given inches to pixels
    /// </summary>
    /// <param name="value">The number of inches</param>
    /// <returns>The equivalent number of pixels</returns>
    public static double InchToPixel(double value) => value * 96;

    /// <summary>
    /// Converts the given picas to pixels
    /// </summary>
    /// <param name="value">The number of picas</param>
    /// <returns>The equivalent number of pixels</returns>
    public static double PicaToPixel(double value) => InchToPixel(value) / 6;

    /// <summary>
    /// Converts the given points to pixels
    /// </summary>
    /// <param name="value">The number of points</param>
    /// <returns>The equivalent number of pixels</returns>
    public static double PointToPixel(double value) => value / (72.0 / 96.0);

    /// <summary>
    /// Converts the given percentage to pixels
    /// </summary>
    /// <param name="value">The percentage of the size</param>
    /// <param name="width">The width of the parent or card</param>
    /// <param name="height">The height of the parent or card</param>
    /// <param name="isWidth">Whether or not the size represents the width or height</param>
    /// <returns>The equivalent number of pixels</returns>
    /// <exception cref="ArgumentException">Thrown if the context of the size is not given correctly</exception>
    public static double PercentToPixel(double value, int? width, int? height, bool? isWidth)
    {
        if (isWidth is null) throw new ArgumentException(ERROR_PERCENT);

        if (isWidth.Value && width is null ||
            !isWidth.Value && height is null) throw new ArgumentException(ERROR_PERCENT);

        return value / 100 * (isWidth.Value ? width!.Value : height!.Value);
    }

    /// <summary>
    /// Converts the given relative percentage to pixels
    /// </summary>
    /// <param name="value">The percentage of the size</param>
    /// <param name="width">The width of the parent or card</param>
    /// <param name="height">The height of the parent or card</param>
    /// <returns>The equivalent number of pixels</returns>
    /// <exception cref="ArgumentException">Thrown if the context of the size is not given correctly</exception>
    public static double RelativePercentageToPixel(double value, int? width, int? height)
    {
        if (width is null || height is null) throw new ArgumentException(ERROR_PERCENT);
        return value / 100 * (width.Value + height.Value) / 2;
    }

    /// <summary>
    /// Converts the given em value to pixels relative to the context's font size
    /// </summary>
    /// <param name="value">The number of ems</param>
    /// <param name="fontSize">The size of the font in the current context</param>
    /// <returns>The equivalent number of pixels</returns>
    public static double EmToPixel(double value, int? fontSize)
    {
        if (fontSize is null) throw new ArgumentException("Cannot use em measurement without a font size");

        return value * fontSize.Value;
    }

    /// <summary>
    /// Represents an available unit of measurement
    /// </summary>
    /// <param name="Type">The type of unit</param>
    /// <param name="Symbol">The symbol that indicates the unit</param>
    public record class CssUnit(
        CardUnitType Type,
        string Symbol);
}
