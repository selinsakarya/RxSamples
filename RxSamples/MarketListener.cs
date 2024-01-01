using System.Text.Json;

namespace RxSamples;

public class MarketListener : IObserver<decimal>
{
    public IDisposable ListenToMarket(Market market)
    {
        return market.Subscribe(this);
    }
    
    public void OnCompleted()
    {
        Console.Write("Completed");
    }

    public void OnError(Exception error)
    {
        Console.Write($"Error {error.Message}");
    }

    public void OnNext(decimal value)
    {
        Console.WriteLine($"Market price: {value}");
    }
}