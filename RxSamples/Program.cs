using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using RxSamples;
using Timer = System.Timers.Timer;

// Example1();
// await Example2();
// Example3();
// Example4();
// BroadCasting();
// ReplaySubject();
// ReplaySubject2();
// BehaviorSubject();
// AsyncSubject();
// SimpleFactoryMethods();
// BlockingNonBlocking();
// TickTock();

void Example1()
{
    Market market = new Market()
    {
        Price = 700
    };

    MarketListener marketListener = new MarketListener();

    IDisposable subscription = marketListener.ListenToMarket(market);

    market.Publish(100);

    subscription.Dispose();
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

void BroadCasting()
{
    // Observable
    Subject<decimal> marketPrice = new Subject<decimal>();

    // Observer of marketPrice
    // Observable
    Subject<decimal> marketPriceConsumer = new Subject<decimal>();

    marketPrice.Subscribe(marketPriceConsumer);

    marketPriceConsumer.Inspect("Market consumer");

    marketPrice.OnNext(100, 200, 150, 500);

    marketPrice.OnCompleted();
}

void ReplaySubject()
{
    ReplaySubject<decimal> marketPrice = new ReplaySubject<decimal>();

    marketPrice.OnNext(200);

    marketPrice.Subscribe(value => Console.WriteLine($"Value received: {value}"));

    marketPrice.OnNext(400);
}

void ReplaySubject2()
{
    TimeSpan timeWindow = TimeSpan.FromMilliseconds(500);

    ReplaySubject<decimal> marketPrice = new ReplaySubject<decimal>(timeWindow);
    // ReplaySubject<decimal> marketPrice = new ReplaySubject<decimal>(2);

    marketPrice.OnNext(200);
    Thread.Sleep(200);

    marketPrice.OnNext(300);
    Thread.Sleep(200);

    marketPrice.OnNext(400);
    Thread.Sleep(200);

    marketPrice.OnNext(500);
    Thread.Sleep(200);

    marketPrice.Subscribe(value => Console.WriteLine($"Value received: {value}"));
}

void BehaviorSubject()
{
    BehaviorSubject<decimal> marketPrice = new BehaviorSubject<decimal>(-1);

    marketPrice.Inspect("Market Price consumer");

    marketPrice.OnNext(300);
}

void AsyncSubject()
{
    AsyncSubject<decimal> marketPrice = new AsyncSubject<decimal>();

    marketPrice.Inspect("AsyncSubject");

    marketPrice.OnNext(300);
    marketPrice.OnNext(400);

    marketPrice.OnCompleted();
}

void SimpleFactoryMethods()
{
    // IObservable<int> obs = Observable.Return(123);
    // IObservable<int> obs = Observable.Empty<int>();
    // IObservable<int> obs = Observable.Never<int>();
    IObservable<int> obs = Observable.Throw<int>(new Exception("oops"));

    obs.Inspect("obs");
}

void BlockingNonBlocking()
{
    IObservable<string> blockingObservable = Blocking();

    IObservable<string> nonBlockingObservable = NonBlocking();

    Console.WriteLine("Start");

    nonBlockingObservable.Inspect("nonBlockingObservable");

    blockingObservable.Inspect("blockingObservable");

    IObservable<string> Blocking()
    {
        ReplaySubject<string> subject = new ReplaySubject<string>();

        subject.OnNext("asd", "xyz");

        subject.OnCompleted();

        Thread.Sleep(3000);

        return subject;
    }

    IObservable<string> NonBlocking()
    {
        return Observable.Create<string>(observer =>
        {
            observer.OnNext("asd", "xyz");

            observer.OnCompleted();

            Thread.Sleep(3000);

            return Disposable.Empty;
        });
    }
}

void TickTock()
{
    var obs = Observable.Create<string>(o =>
    {
        Timer timer = new Timer(1000);

        timer.Elapsed += (sender, e) => o.OnNext($"tick {e.SignalTime}");
    
        timer.Elapsed += (sender, e) => Console.WriteLine($"tock {e.SignalTime}");
    
        timer.Start();

        return () => timer.Dispose();
    });

    IDisposable sub = obs.Inspect("timer");

    Console.ReadLine();

    sub.Dispose();

    Console.ReadLine();
}
