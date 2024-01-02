using System.Collections.Immutable;
using System.Reactive.Disposables;

namespace RxSamples;

public class Market : IObservable<decimal>
{
    public decimal Price { get; set; }

    private ImmutableHashSet<IObserver<decimal>> _observers = ImmutableHashSet<IObserver<decimal>>.Empty;

    public IDisposable Subscribe(IObserver<decimal> observer)
    {
        ArgumentNullException.ThrowIfNull(observer);

        _observers = _observers.Add(observer);

        return Disposable.Create(() =>
        {
            Console.WriteLine("Disposing");
            _observers = _observers.Remove(observer);
        });
    }

    public void Publish(decimal price)
    {
        foreach (var observer in _observers)
        {
            observer.OnNext(price);
        }
    }
}

public class Market2
{
    private decimal _price;

    public decimal Price
    {
        get => _price;
        set => _price = value;
    }

    public event EventHandler<decimal> PriceChanged;

    public void OnPriceChanged(decimal price)
    {
        Price = price;

        PriceChanged?.Invoke(this, price);
    }
}