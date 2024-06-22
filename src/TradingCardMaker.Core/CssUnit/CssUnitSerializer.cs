namespace TradingCardMaker.Core.CssUnit;

internal class CssUnitSerializer : JsonConverter<CssUnit>
{
    public override CssUnit Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            if (string.IsNullOrEmpty(value)) return CssUnit.Zero;
            return CssUnit.Parse(value);
        }

        var node = JsonNode.Parse(ref reader);
        if (node is null) return CssUnit.Zero;

        var unit = (double?)node["value"]?.AsValue();
        if (unit is null) return CssUnit.Zero;

        var type = node["type"]?.ToString();
        if (type is null) return CssUnit.Zero;

        if (!Enum.TryParse<CssUnitType>(type, true, out var cardUnit))
            cardUnit = CssUnitType.Pixel;

        return new CssUnit(cardUnit, unit.Value);
    }

    public override void Write(Utf8JsonWriter writer, CssUnit value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
