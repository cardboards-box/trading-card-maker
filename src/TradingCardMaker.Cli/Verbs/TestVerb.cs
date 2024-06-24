
using TradingCardMaker.Drawing.Loading;

namespace TradingCardMaker.Cli.Verbs;

[Verb("test", HelpText = "Just doing some testing")]
public class TestVerOptions
{

}

internal class TestVerb(
    ILogger<TestVerb> logger,
    ICardLoaderService _cards) : BooleanVerb<TestVerOptions>(logger)
{
    public override async Task<bool> Execute(TestVerOptions options, CancellationToken token)
    {
        const string PATH = @"C:\Users\Cardboard\Desktop\example-cards\faceit-trading-card";

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
