﻿using System.Reactive.Subjects;
using RxSamples;

// Example1();

// await Example2();

// Example3();

Example4();

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
    // marketPrice.OnError(new Exception("Error message"));

    marketPrice.OnNext(500);
}

void Example3()
{
    Subject<decimal> marketPrice = new Subject<decimal>();

    marketPrice.OnNext(300);

    marketPrice.Subscribe(
        value => Console.WriteLine($"Value: {value}"),
        error => Console.WriteLine($"Error: {error.Message}"),
        () => Console.WriteLine("Completed")
    );

    marketPrice.OnNext(600);

    marketPrice.OnCompleted();
    // marketPrice.OnError(new Exception("Error message"));

    marketPrice.OnNext(500);

}

void Example4()
{
    Subject<decimal> marketPrice = new Subject<decimal>();

    IDisposable subscription = marketPrice.Subscribe(
        value => Console.WriteLine($"Value: {value}"),
        error => Console.WriteLine($"Error: {error.Message}"),
        () => Console.WriteLine("Completed")
    );

    marketPrice.OnNext(3000);

    subscription.Dispose();

    marketPrice.OnNext(1000);
}