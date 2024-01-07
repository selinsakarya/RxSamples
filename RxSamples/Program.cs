using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
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
// Range();
// Generate();
// Interval();
// Timer();
// LazyObservable();
// FromEventPattern();
// FromTask();
// FromEnumerable();
// SequenceFilteringWhere();
// SequenceFilteringSelect();
// DistinctUntilChanged();
// While();
// SkipUntil();
// Any();
// All();
// DefaultIfEmpty();
// ElementAt();
// SequenceEqual();
// OfTypeAndCast();
// Time();
// Materialize();
// SelectMany();
// Count();
// MinMaxAvg();
// FirstOrDefaultAsync();
// Aggregate();
// Scan();
// ErrorHandling();
// OnErrorResumeNext();
// SucceedAfter(3).Retry(4).Inspect("retry");
// SucceedAfter(4).Retry(3).Inspect("retry");
// CombineLatest();
// Zip();
// AndThenWhen();
// StartWithConcatRepeat();
// Amb();
// Merge();

IObservable<int> SucceedAfter(int attempts)
{
    int count = 0;

    return Observable.Create<int>(o =>
    {
        Console.WriteLine((count > 0 ? "Ret" : "T") + "rying to do the work");

        if (count++ < attempts)
        {
            Console.WriteLine("Failed");
            o.OnError(new Exception());
        }
        else
        {
            Console.WriteLine("succeeded");
            o.OnNext(count);
            o.OnCompleted();
        }

        return Disposable.Empty;
    });
}

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

void Range()
{
    IObservable<int> tenToTwenty = Observable.Range(10, 11);

    tenToTwenty.Inspect("tenToTwenty");
}

void Generate()
{
    IObservable<string> generated = Observable.Generate(1,
        value => value < 100,
        value => value * value + 1,
        value => $"[val: {value}]");

    generated.Inspect("generated");
}

void Interval()
{
    IObservable<long> interval = Observable.Interval(TimeSpan.FromMilliseconds(500));

    using (interval.Inspect("interval"))
    {
        Console.ReadKey();
    }
}

void Timer()
{
    // IObservable<long> timer = Observable.Timer(TimeSpan.FromSeconds(2));

    IObservable<long> timer = Observable.Timer(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(2));

    timer.Inspect("timer");

    Console.ReadLine();
}

void LazyObservable()
{
    IObservable<Unit> start = Observable.Start(() =>
    {
        Console.WriteLine("Starting work");

        for (int i = 0; i < 10; i++)
        {
            Thread.Sleep(200);
            Console.Write(".");
        }
    });

    for (int i = 0; i < 10; i++)
    {
        Thread.Sleep(200);
        Console.Write("-");
    }

    start.Inspect("start");

    Console.ReadLine();
}

void FromEventPattern()
{
    Market2 market = new Market2();

    IObservable<EventPattern<decimal>> priceChanges = Observable.FromEventPattern<decimal>(
        h => market.PriceChanged += h,
        h => market.PriceChanged -= h
    );

    priceChanges.Subscribe(x => Console.WriteLine(x.EventArgs));

    market.OnPriceChanged(10);
    market.OnPriceChanged(20);
    market.OnPriceChanged(30);
}

void FromTask()
{
    Task<string> t = Task.Factory.StartNew(() => "Test");

    IObservable<string> obs = t.ToObservable();

    obs.Inspect("obs");
}

void FromEnumerable()
{
    List<int> items = new List<int>() { 10, 20, 30 };

    IObservable<int> obs = items.ToObservable();

    obs.Inspect("obs");
}

void SequenceFilteringWhere()
{
    Observable.Range(0, 1000)
        .Where(i => i % 9 == 0)
        .Inspect("where");
}

void SequenceFilteringSelect()
{
    Observable.Range(-10, 21)
        .Select(x => x * x)
        .Distinct()
        .Inspect("select distinct");
}

void DistinctUntilChanged()
{
    new List<int>() { 1, 1, 2, 3, 3, 4, 2 }
        .ToObservable()
        .DistinctUntilChanged()
        .Inspect("DistinctUntilChanged");
}

void While()
{
    Observable.Range(-10, 21)
        .SkipWhile(x => x < 0)
        .TakeWhile(x => x < 6)
        .Inspect("while");
}

void SkipUntil()
{
    Subject<decimal> stockPrices = new Subject<decimal>();
    Subject<decimal> optionPrices = new Subject<decimal>();

    optionPrices.SkipUntil(stockPrices).Inspect("optionPrices");

    optionPrices.OnNext(1, 2, 3);

    stockPrices.OnNext(10, 20);

    optionPrices.OnNext(4, 5, 6);
}

void Any()
{
    Subject<int> subject = new Subject<int>();

    subject.Any(x => x > 1).Inspect("any");

    // subject.OnNext(2,3);

    subject.OnCompleted();
}

void All()
{
    List<int> values = new List<int>() { 1, 2, 3, 4, 5 };

    values.ToObservable()
        .All(x => x > 0)
        .Inspect("all");
}

void DefaultIfEmpty()
{
    var subject = new Subject<float>();

    subject.DefaultIfEmpty(0.99f)
        .Inspect("DefaultIfEmpty");

    subject.OnCompleted();
}

void ElementAt()
{
    IObservable<int> numbers = Observable.Range(1, 10);

    numbers.ElementAt(5).Inspect("ElementAt");

    numbers.ElementAt(15).Inspect("ElementAt");
}

void SequenceEqual()
{
    Subject<int> seq1 = new Subject<int>();
    Subject<int> seq2 = new Subject<int>();

    seq1.Inspect("seq1");
    seq1.Inspect("seq2");

    seq1.SequenceEqual(seq2)
        .Inspect("SequenceEqual");

    seq1.OnNext(2);
    seq2.OnNext(2);

    seq1.OnCompleted();
    seq2.OnCompleted();
}

void OfTypeAndCast()
{
    Subject<object> subj = new Subject<object>();

    subj.OfType<float>().Inspect("OfType");
    subj.Cast<float>().Inspect("Cast");

    subj.OnNext(1.2f, 2, 3.0);
}

void Time()
{
    IObservable<long> seq = Observable.Interval(TimeSpan.FromSeconds(1));

    // seq.Timestamp().Inspect("Timestamp");
    seq.TimeInterval().Inspect("TimeInterval");

    Console.ReadLine();
}

void Materialize()
{
    IObservable<int> seq = Observable.Range(0, 4);

    seq.Materialize().Inspect("Materialize");
}

void SelectMany()
{
    // 1 1 2 1 2 3 1 2 3 4

    Observable.Range(1, 4, Scheduler.Immediate)
        .SelectMany(x => Observable.Range(1, x, Scheduler.Immediate))
        .Inspect("SelectMany");
}

void Count()
{
    IObservable<int> numbers = Observable.Range(1, 5);

    numbers.Inspect("values");

    numbers.Count().Inspect("count");
}

void MinMaxAvg()
{
    Subject<int> subj = new Subject<int>();

    subj.Inspect("subj");
    subj.Min().Inspect("Min");
    subj.Max().Inspect("Max");
    subj.Average().Inspect("Average");

    subj.OnNext(1, 2, 7, 5);
    subj.OnCompleted();
}

void FirstOrDefaultAsync()
{
    ReplaySubject<int> replay = new ReplaySubject<int>();

    replay.OnNext(-1);
    replay.OnNext(2);
    replay.OnCompleted();

    replay.FirstOrDefaultAsync(i => i > 0).Inspect("FirstOrDefaultAsync");
}

void Aggregate()
{
    Subject<double> subj = new Subject<double>();

    int power = 1;

    subj.Aggregate(0.0, (p, c) => p + Math.Pow(c, power++)).Inspect("Aggregate");

    subj.OnNext(1, 2, 4).OnCompleted();
}

void Scan()
{
    Subject<int> subj = new Subject<int>();

    subj.Scan(0.0, (p, c) => p + c).Inspect("Scan");

    subj.OnNext(1, 2, 3, 4, 5);
}

void ErrorHandling()
{
    Subject<int> subj = new Subject<int>();
    IObservable<int> fallback = Observable.Range(1, 3);

    // subj.Catch(Observable.Range(1, 3)).Inspect("subj");
    subj
        .Catch<int, ArgumentException>(ex => Observable.Return(111))
        .Catch(fallback).Inspect("subj");

    subj.OnNext(32);
    subj.OnError(new ArgumentException("arg"));
    subj.OnError(new Exception("oops"));
}

void OnErrorResumeNext()
{
    Subject<char> seq1 = new Subject<char>();
    Subject<char> seq2 = new Subject<char>();

    seq1.OnErrorResumeNext(seq2).Inspect("OnErrorResumeNext");

    seq1.OnNext('a', 'b', 'c').OnError(new Exception());
    seq2.OnNext('d', 'e', 'f').OnCompleted();
}

void CombineLatest()
{
    BehaviorSubject<bool> mechanical = new BehaviorSubject<bool>(true);
    BehaviorSubject<bool> electrical = new BehaviorSubject<bool>(true);
    BehaviorSubject<bool> electronic = new BehaviorSubject<bool>(true);

    mechanical.Inspect("mechanical");
    electrical.Inspect("electrical");
    electronic.Inspect("electronic");

    Observable.CombineLatest(mechanical, electrical, electronic)
        .Select(values => values.All(x => x))
        .Inspect("Is the system ok?");

    electronic.OnNext(false);
}

void Zip()
{
    IObservable<int> digits = Observable.Range(10, 30);
    IObservable<char> letters = Observable.Range(0, 10)
        .Select(x => (char)('A' + x));

    letters.Zip(digits, (letter, digit) => $"{letter}{digit}")
        .Inspect("zip");
}

void AndThenWhen()
{
    IObservable<char> punctuation = "£#$½$½#§()".ToCharArray().ToObservable();
    IObservable<int> digits = Observable.Range(10, 30);
    IObservable<char> letters = Observable.Range(0, 10).Select(x => (char)('A' + x));

    Observable.When(digits.And(letters).And(punctuation)
        .Then((digit, letter, symbol) => $"{digit}{letter}{symbol}")).Inspect("and-then-when");
}

void StartWithConcatRepeat()
{
    IObservable<int> s1 = Observable.Range(1, 3);
    IObservable<int> s2 = Observable.Range(4, 3);

// s1.Concat(s2).Inspect("concat");
// s1.Repeat(3).Inspect("repeat");
    s1.StartWith(2, 1, 0).Inspect("startwith");
}

void Amb()
{
    Subject<int> seq1 = new Subject<int>();
    Subject<int> seq2 = new Subject<int>();
    Subject<int> seq3 = new Subject<int>();

    seq1.Amb(seq2).Amb(seq3).Inspect("Amb");

    seq2.OnNext(10);

    seq1.OnNext(1);
    seq1.OnNext(2);
    seq1.OnNext(3);

    seq2.OnNext(20);
    seq2.OnNext(30);

    seq3.OnNext(100);
    seq3.OnNext(200);
    seq3.OnNext(300);
}

void Merge()
{
    Subject<long> foo = new Subject<long>();
    Subject<long> bar = new Subject<long>();
    IObservable<long> baz = Observable.Interval(TimeSpan.FromSeconds(0.5)).Take(5);

    foo.Merge(bar).Merge(baz).Inspect("merge");

    foo.OnNext(100);

    Thread.Sleep(1000);

    bar.OnNext(10);

    Thread.Sleep(1000);

    foo.OnNext(1000);

    Thread.Sleep(1000);
}