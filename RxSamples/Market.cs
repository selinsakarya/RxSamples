namespace RxSamples;

public class Market : IObservable<Market>
{
    public decimal Price { get; set; }

    public IDisposable Subscribe(IObserver<Market> observer)
    {
        observer.OnNext(this);
       
        return null;
    }
}