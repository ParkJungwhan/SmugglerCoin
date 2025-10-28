using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SmugglerCoin.Models;

namespace SmugglerCoin.UpbitModels.Models;

public class UpbitManager   // TODO: consider enforcing singleton semantics
{
    public Dictionary<string, int> CurrentStatus;
    public ILogger<UpbitManager>? logger;

    private const float Loop_Seconds = 10f;     // TODO: verify intended interval
    private Smuggler PC;
    private readonly ConcurrentQueue<string> qWalletResult;
    private readonly object _marketLock = new();
    private List<MarketAllModel> _marketPairs = new();

    public UpbitManager()
    {
        qWalletResult = new ConcurrentQueue<string>();
        PC = new Smuggler();
        InitManager();
    }

    public UpbitManager(ILogger<UpbitManager> logger, Smuggler pc)
    {
        qWalletResult = new ConcurrentQueue<string>();
        PC = pc;
        this.logger = logger;
        InitManager();
    }

    public IReadOnlyList<MarketAllModel> MarketPairs
    {
        get
        {
            lock (_marketLock)
            {
                return _marketPairs.ToList();
            }
        }
    }

    public void UpdateMarketPairs(IEnumerable<MarketAllModel> pairs)
    {
        ArgumentNullException.ThrowIfNull(pairs);

        var snapshot = pairs as List<MarketAllModel> ?? pairs.ToList();
        lock (_marketLock)
        {
            _marketPairs = snapshot;
        }

        logger?.LogInformation($"{DateTime.Now}\t[Set]MarketPairs Count: {_marketPairs.Count}");
    }

    private async Task InitManager()
    {
        CurrentStatus = new Dictionary<string, int>();

        var manager = new CoroutineManager();
        logger?.LogDebug("All Coroutines Started.");

        while (true)
        {
            //manager.Start(async token => await PrintMessage("A", 10, 0.1f, token));
            //manager.Start(async token => await PrintMessage("B", 5, 0.2f, token));
            manager.Start(async token => await ActionCoroutine(ProcessWalletStatus, token));    // TODO: append additional actions here

            await manager.RunAllAsync();
            Task.Delay(100).Wait();         // temporary delay
        }

        // ReSharper disable once HeuristicUnreachableCode
        logger?.LogDebug("All coroutines finished.");
    }

    private async Task ActionCoroutine(Action act, CancellationToken token, int nLoopCount = 10)
    {
        int counter = 0;
        while (!token.IsCancellationRequested && counter < nLoopCount)
        {
            logger?.LogDebug($"Counter: {counter++}");
            act();
            await Wait.ForSeconds(1.0f, token);
        }
    }

    public void EnqueueWalletStatus(string json)
    {
        logger?.LogDebug($"{DateTime.Now}\t[Get]EnqueueWalletStatus");

        qWalletResult.Enqueue(json);
    }

    private void ProcessWalletStatus()
    {
        logger?.LogDebug($"{DateTime.Now}\t Start ProcessWalletStatus");

        while (qWalletResult
            .TryDequeue(out var json))
        {
            // TODO: add JSON processing logic
            logger?.LogDebug($"{DateTime.Now}\tProcessing wallet status");
        }

        logger?.LogDebug($"{DateTime.Now}\t Finish ProcessWalletStatus");
    }
}