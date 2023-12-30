using System.Text.Json;

namespace RxSamples;

public class MarketListener : IObserver<Market>
{
    public void ListenToMarket(Market market)
    {
        market.Subscribe(this);
    }
    
    public void OnCompleted()
    {
        Console.Write("Completed");
    }

    public void OnError(Exception error)
    {
        Console.Write($"Error {error.Message}");
    }

    public void OnNext(Market value)
    {
        Console.WriteLine($"Market Json: {JsonSerializer.Serialize(value)}");
    }
}