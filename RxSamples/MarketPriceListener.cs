namespace RxSamples;

public class MarketPriceListener : IObserver<decimal>
{
    public void OnCompleted()
    {
        Console.WriteLine("MarketPriceListener OnCompleted");
    }

    public void OnError(Exception error)
    {
        Console.WriteLine($"MarketPriceListener OnError error: {error.Message}");
    }

    public void OnNext(decimal value)
    {
        Console.WriteLine($"MarketPriceListener OnNext value: {value}");
    }
}