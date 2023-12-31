using System.Reactive.Disposables;

namespace RxSamples;

public class Market : IObservable<Market>
{
    public decimal Price { get; set; }

    private readonly List<IObserver<Market>> _observers = new();

    public IDisposable Subscribe(IObserver<Market> observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        _observers.Add(observer);

        return Disposable.Empty;
    }

    public void UpdatePrice(decimal price)
    {
        Price = price;

        foreach (IObserver<Market> observer in _observers)
        {
            observer.OnNext(this);
        }
    }
}