using System.Reactive.Subjects;
using RxSamples;

// Example1();

await Example2();

void Example1()
{
    Market market = new Market()
    {
        Price = 70000
    };

    MarketListener marketListener = new MarketListener();

    marketListener.ListenToMarket(market);

    market.UpdatePrice(100);
}

async Task Example2()
{
    Subject<decimal> marketPrice = new Subject<decimal>();

    MarketPriceListener marketPriceListener = new MarketPriceListener();

    MarketPriceListener marketPriceListener2 = new MarketPriceListener();

    marketPrice.Subscribe(marketPriceListener);

    marketPrice.Subscribe(marketPriceListener2);

    marketPrice.OnNext(400);

    await Task.Run(async () =>
    {
        await Task.Delay(2000);
        marketPrice.OnNext(2312);
    });
    
    marketPrice.OnCompleted();
    // marketPrice.OnError(new Exception("Error"));

    marketPrice.OnNext(500);
}