using RxSamples;

Market market = new Market()
{
    Price = 70000
};

MarketListener marketListener = new MarketListener();

marketListener.ListenToMarket(market);
