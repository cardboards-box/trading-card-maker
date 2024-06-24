using TradingCardMaker.Cli.Verbs;

return await new ServiceCollection()
    .AddCore()
    .AddDrawing()
    .AddModels()
    .AddTemplating()
    .Cli(c => c.Add<TestVerb>());