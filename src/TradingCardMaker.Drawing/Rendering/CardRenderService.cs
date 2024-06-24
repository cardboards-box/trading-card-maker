namespace TradingCardMaker.Drawing.Rendering;

using Drawing;
using Loading;

public interface ICardRenderService
{

}

internal class CardRenderService : ICardRenderService
{
    public async Task<Stream> Render(LoadedCardSet set, LoadedCard card)
    {
        var rootScope = new RenderScope();
        var context = new RenderContext
        {
            CardSet = set,
            Card = card,
            ScopeStack = [rootScope]
        };

        if (card.Runner is not null)
        {
            var result = await card.Runner.Execute(set);
            rootScope.Set(result);
        }

        throw new NotImplementedException();
    }
}
