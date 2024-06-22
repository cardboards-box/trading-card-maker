namespace TradingCardMaker.Tests.ModelsTests;

using Core;

[TestClass]
public class CardSizeUtilityTests : TestSetup
{
    [TestMethod]
    public void DeserializeTests()
    {
        (string test, CardUnit output)[] tests = 
        [
            ("-2.3cm", new(CardUnitType.Centimeter, -2.3)),
            ("3.3mm", new(CardUnitType.Millimeter, 3.3)),
            ("1q", new(CardUnitType.QuarterMillimeter, 1)),
            ("00100.000in", new(CardUnitType.Inch, 100)),
            ("1.2pc", new(CardUnitType.Pica, 1.2)),
            ("1.2pt", new(CardUnitType.Point, 1.2)),
            ("1.2%", new(CardUnitType.Percentage, 1.2)),
            ("1.2em", new(CardUnitType.Em, 1.2)),
            ("1.2vh", new(CardUnitType.ViewHeight, 1.2)),
            ("1.2vw", new(CardUnitType.ViewWidth, 1.2)),
            ("1.2rp", new(CardUnitType.RelativePercentage, 1.2)),
            ("1.2", new(CardUnitType.Pixel, 1.2))
        ];

        foreach (var (test, output) in tests)
        {
            var parsed = CardUnit.Parse(test);
            Assert.AreEqual(output.Type, parsed.Type, $"Type Test: {test}");
            Assert.AreEqual(output.Value, parsed.Value, $"Value Test: {test}");
        }
    }

    [TestMethod]
    public void SerializeTests()
    {
        (CardUnit test, string output)[] tests =
        [
            (new(CardUnitType.Centimeter, -2.3), "-2.3cm"),
            (new(CardUnitType.Millimeter, 3.3), "3.3mm"),
            (new(CardUnitType.QuarterMillimeter, 1), "1q"),
            (new(CardUnitType.Inch, 100), "100in"),
            (new(CardUnitType.Pica, 1.2), "1.2pc"),
            (new(CardUnitType.Point, 1.2), "1.2pt"),
            (new(CardUnitType.Percentage, 1.2), "1.2%"),
            (new(CardUnitType.Em, 1.2), "1.2em"),
            (new(CardUnitType.ViewHeight, 1.2), "1.2vh"),
            (new(CardUnitType.ViewWidth, 1.2), "1.2vw"),
            (new(CardUnitType.RelativePercentage, 1.2), "1.2rp"),
            (new(CardUnitType.Pixel, 1.2), "1.2px")
        ];

        foreach(var (test, output) in tests)
        {
            var serialized = test.Serialize();
            Assert.AreEqual(output, serialized, $"Serialize Test: {test.Type} - {test.Value}");
        }
    }

    [TestMethod]
    public void CalculatePixelTests()
    {
        var root = new SizeContext(0, 0, 200, 100, 15);

        var context = root.GetContext(0, 0, 100, 50);

        (string test, int output, bool? isWidth)[] tests =
        [
            ("12px", 12, null),
            ("22cm", 831, null),
            ("-30.50mm", -115, null),
            ("00999.00q", 944, null),
            ("-.4in", -38, null),
            ("1,000.30pc", 16005, null),
            ("-69.69pt", -93, null),
            ("100vw", 200, null),
            ("80vw", 160, null),
            ("50vh", 50, null),
            ("200vh", 200, null),
            ("20em", 300, null),
            ("30%", 30, true),
            ("60%", 30, false)
        ];

        foreach(var (test, output, isWidth) in tests)
        {
            var card = CardUnit.Parse(test);
            var px = card.Pixels(context, isWidth);
            Assert.AreEqual(output, px, $"Pixel Test: {test}");
        }
    }

    [TestMethod]
    public void JsonTests()
    {
        var values = new[]
        {
            "-2.3cm",
            "3.3mm",
            "1q",
            "100in",
            "1.2pc",
            "1.2pt",
            "1.2%",
            "1.2em",
            "1.2vh",
            "1.2vw",
            "1.2rp",
            "1.2px"
        }.Select(CardUnit.Parse);

        foreach(var card in values)
        {
            var testObj = new JsonCardSizeTest { Size = card };
            var json = JsonSerializer.Serialize(testObj);
            var obj = JsonSerializer.Deserialize<JsonCardSizeTest>(json);

            Assert.AreEqual(card.ToString(), obj?.Size?.Serialize(), $"Json Test: {card}");
        }
    }

    [TestMethod]
    public void JsonNullTests()
    {
        var testObj = new JsonCardSizeTest { Size = null };
        var json = JsonSerializer.Serialize(testObj);
        var obj = JsonSerializer.Deserialize<JsonCardSizeTest>(json);

        Assert.IsNotNull(obj, "Json Null Test - Parent");
        Assert.IsNull(obj.Size, "Json Null Test - Value");
    }

    internal class JsonCardSizeTest
    {
        [JsonPropertyName("size")]
        public CardUnit? Size { get; set; }
    }
}
