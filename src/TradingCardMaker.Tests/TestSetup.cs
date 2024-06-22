namespace TradingCardMaker.Tests;

using Templating;
using Models;

public abstract class TestSetup
{
    private IServiceProvider? _services;

    private IServiceProvider GetServiceProvider()
    {
        if (_services is not null) return _services;

        return _services = new ServiceCollection()
            .AddTemplating()
            .AddModels()
            .BuildServiceProvider();
    }

    public T GetService<T>() where T : notnull
    {
        return GetServiceProvider().GetRequiredService<T>();
    }
}
