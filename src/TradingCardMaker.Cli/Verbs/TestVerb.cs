namespace TradingCardMaker.Cli.Verbs;

using Drawing.Loading;
using Drawing.Rendering;

[Verb("test", HelpText = "Just doing some testing")]
public class TestVerOptions
{

}

internal class TestVerb(
    ILogger<TestVerb> logger,
    ICardLoaderService _cards,
    ISvgService _svg) : BooleanVerb<TestVerOptions>(logger)
{
    public const string PATH = @"C:\Users\Cardboard\Desktop\example-cards\faceit-trading-card";

    public async Task SvgTest()
    {
        var assetsDir = Path.Combine(PATH, @"assets\logos");
        var files = Directory.GetFiles(assetsDir, "*.svg");

        foreach (var file in files)
        {
            var name = Path.GetFileName(file);
            using var io = File.Create(name + ".png");
            await _svg.SaveBitmap(io, file);
        }
    }

    public override async Task<bool> Execute(TestVerOptions options, CancellationToken token)
    {
        await SvgTest();

        var loader = await _cards.Load(PATH);

        var card = loader.BackFace;
        if (card is null || card.Runner is null)
        {
            _logger.LogWarning("Card face is null");
            return false;
        }

        var result = await card.Runner.Execute(loader);

        _logger.LogInformation("Hello world");
        return true;
    }
}
