namespace TradingCardMaker.Core.Helpers;

internal class CardSizeSerializer : JsonConverter<CardUnit>
{
    public override CardUnit Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            var value = reader.GetString();
            if (string.IsNullOrEmpty(value)) return CardUnit.Zero;
            return CardUnit.Parse(value);
        }

        var node = JsonNode.Parse(ref reader);
        if (node is null) return CardUnit.Zero;

        var unit = (double?)node["value"]?.AsValue();
        if (unit is null) return CardUnit.Zero;

        var type = node["type"]?.ToString();
        if (type is null) return CardUnit.Zero;

        if (!Enum.TryParse<CardUnitType>(type, true, out var cardUnit))
            cardUnit = CardUnitType.Pixel;

        return new CardUnit(cardUnit, unit.Value);
    }

    public override void Write(Utf8JsonWriter writer, CardUnit value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
