﻿namespace TradingCardMaker.Models.Drawing;

using Helpers;
using Helpers.Serializers;

/// <summary>
/// Represents a unit of measurement of something on a card
/// </summary>
/// <param name="Type">The type of unit of measurement</param>
/// <param name="Value">The value of the measurement</param>
[JsonConverter(typeof(CardSizeSerializer))]
public record struct CardUnit(
    [property: JsonPropertyName("type")] CardUnitType Type,
    [property: JsonPropertyName("value")] double Value)
{
    /// <summary>
    /// A size of zero
    /// </summary>
    public static CardUnit Zero { get; } = new(CardUnitType.Pixel, 0);

    /// <summary>
    /// 100% of the parent size
    /// </summary>
    public static CardUnit Fill { get; } = new(CardUnitType.Percentage, 100);

    /// <summary>
    /// 100% of the parent width
    /// </summary>
    public static CardUnit FillWidth { get; } = new(CardUnitType.ViewWidth, 100);

    /// <summary>
    /// 100% of the parent height
    /// </summary>
    public static CardUnit FillHeight { get; } = new(CardUnitType.ViewHeight, 100);

    /// <summary>
    /// Gets the equivalent number of pixels for this unit of measurement
    /// </summary>
    /// <param name="context">The context of the parent</param>
    /// <param name="isWidth">Whether or not the unit is for widths or heights (or null if the context isn't known)</param>
    /// <returns>The number of pixels</returns>
    public readonly int Pixels(CardUnitContext context, bool? isWidth) => CssUnitHelper.GetPixels(this, context, isWidth);

    /// <summary>
    /// Converts the unit of measurement to a string
    /// </summary>
    /// <returns>The string version of the measurement</returns>
    public readonly string Serialize() => CssUnitHelper.SerializeUnit(this);

    /// <summary>
    /// Converts the unit of measurement to a string
    /// </summary>
    /// <returns>The serialized unit of measurement</returns>
    public override readonly string ToString() => Serialize();

    /// <summary>
    /// Converts the string to a unit of measurement
    /// </summary>
    /// <param name="input">The string version of the measurement</param>
    /// <returns>The parsed card size</returns>
    public static CardUnit Parse(string input) => CssUnitHelper.ParseUnit(input);

    /// <summary>
    /// Converts the unit of measurement to a string
    /// </summary>
    /// <param name="size">The unit of measurement</param>
    public static implicit operator string(CardUnit size) => size.Serialize();

    /// <summary>
    /// Converts the string to a unit of measurement
    /// </summary>
    /// <param name="input">The string version of the measurement</param>
    public static implicit operator CardUnit(string input) => Parse(input);
}