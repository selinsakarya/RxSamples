using System.Reactive.Subjects;
using RxSamples;

Example1();

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




