using TradingCardMaker.Cli.Verbs;

return await new ServiceCollection()
    .AddCore()
    .AddDrawing()
    .AddModels()
    .AddTemplating()
    .AddSerilog()
    .Cli(c => c.Add<TestVerb>());