namespace TradingCardMaker.Tests.ModelsTests;

using Core;
using TradingCardMaker.Core.CssUnit;

[TestClass]
public class CssUnitUtilityTests : TestSetup
{
    [TestMethod]
    public void DeserializeTests()
    {
        (string test, CssUnit output)[] tests = 
        [
            ("-2.3cm", new(CssUnitType.Centimeter, -2.3)),
            ("3.3mm", new(CssUnitType.Millimeter, 3.3)),
            ("1q", new(CssUnitType.QuarterMillimeter, 1)),
            ("00100.000in", new(CssUnitType.Inch, 100)),
            ("1.2pc", new(CssUnitType.Pica, 1.2)),
            ("1.2pt", new(CssUnitType.Point, 1.2)),
            ("1.2%", new(CssUnitType.Percentage, 1.2)),
            ("1.2em", new(CssUnitType.Em, 1.2)),
            ("1.2vh", new(CssUnitType.ViewHeight, 1.2)),
            ("1.2vw", new(CssUnitType.ViewWidth, 1.2)),
            ("1.2rp", new(CssUnitType.RelativePercentage, 1.2)),
            ("1.2", new(CssUnitType.Pixel, 1.2))
        ];

        foreach (var (test, output) in tests)
        {
            var parsed = CssUnit.Parse(test);
            Assert.AreEqual(output.Type, parsed.Type, $"Type Test: {test}");
            Assert.AreEqual(output.Value, parsed.Value, $"Value Test: {test}");
        }
    }

    [TestMethod]
    public void SerializeTests()
    {
        (CssUnit test, string output)[] tests =
        [
            (new(CssUnitType.Centimeter, -2.3), "-2.3cm"),
            (new(CssUnitType.Millimeter, 3.3), "3.3mm"),
            (new(CssUnitType.QuarterMillimeter, 1), "1q"),
            (new(CssUnitType.Inch, 100), "100in"),
            (new(CssUnitType.Pica, 1.2), "1.2pc"),
            (new(CssUnitType.Point, 1.2), "1.2pt"),
            (new(CssUnitType.Percentage, 1.2), "1.2%"),
            (new(CssUnitType.Em, 1.2), "1.2em"),
            (new(CssUnitType.ViewHeight, 1.2), "1.2vh"),
            (new(CssUnitType.ViewWidth, 1.2), "1.2vw"),
            (new(CssUnitType.RelativePercentage, 1.2), "1.2rp"),
            (new(CssUnitType.Pixel, 1.2), "1.2px")
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
            var card = CssUnit.Parse(test);
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
        }.Select(CssUnit.Parse);

        foreach(var card in values)
        {
            var testObj = new JsonCssUnitTest { Size = card };
            var json = JsonSerializer.Serialize(testObj);
            var obj = JsonSerializer.Deserialize<JsonCssUnitTest>(json);

            Assert.AreEqual(card.ToString(), obj?.Size?.Serialize(), $"Json Test: {card}");
        }
    }

    [TestMethod]
    public void JsonNullTests()
    {
        var testObj = new JsonCssUnitTest { Size = null };
        var json = JsonSerializer.Serialize(testObj);
        var obj = JsonSerializer.Deserialize<JsonCssUnitTest>(json);

        Assert.IsNotNull(obj, "Json Null Test - Parent");
        Assert.IsNull(obj.Size, "Json Null Test - Value");
    }

    internal class JsonCssUnitTest
    {
        [JsonPropertyName("size")]
        public CssUnit? Size { get; set; }
    }
}
