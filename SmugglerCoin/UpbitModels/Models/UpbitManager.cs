using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using SmugglerCoin.Models;

namespace SmugglerCoin.UpbitModels.Models;

public class UpbitManager   // singleton 보장은...?
{
    public Dictionary<string, int> CurrentStatus;
    public ILogger<UpbitManager> logger;

    private const float Loop_Seconds = 10f;     //1ms 단위
    private Smuggler PC;
    private ConcurrentQueue<string> qWalletResult;

    public UpbitManager()
    {
        InitManager();
    }

    public UpbitManager(ILogger<UpbitManager> _logger, Smuggler pc) : base()
    {
        this.PC = pc;
        this.logger = _logger;

        InitManager();
    }

    private async Task InitManager()
    {
        CurrentStatus = new Dictionary<string, int>();

        if (PC == null) PC = new Smuggler();
        qWalletResult = new ConcurrentQueue<string>();

        var manager = new CoroutineManager();
        logger.LogDebug("All Coroutines Started.");

        while (true)
        {
            //manager.Start(async token => await PrintMessage("A", 10, 0.1f, token));
            //manager.Start(async token => await PrintMessage("B", 5, 0.2f, token));
            manager.Start(async token => await ActionCoroutine(ProcessWalletStatus, token));    // 여기서 Action 추가하면서 진행해야할듯...?

            await manager.RunAllAsync();
            Task.Delay(100).Wait();         // 텀 주기, 잠시 대기
        }
        logger.LogDebug("All coroutines finished.");
    }

    //private async Task PrintMessage(string label, int count, float delaySeconds, CancellationToken token)
    //{
    //    for (int i = 0; i < count; i++)
    //    {
    //        Console.WriteLine($"{label}: Step {i}");
    //        ProcessWalletStatus();
    //        await Wait.ForSeconds(delaySeconds, token);
    //    }

    //    Console.WriteLine($"{label} finished.");
    //}

    private async Task ActionCoroutine(Action act, CancellationToken token, int nLoopCount = 10)
    {
        int counter = 0;
        while (!token.IsCancellationRequested && counter < nLoopCount)
        {
            logger.LogDebug($"Counter: {counter++}");
            act();
            await Wait.ForSeconds(1.0f, token);
        }
    }

    public void EnqueueWalletStatus(string json)
    {
        logger.LogDebug($"{DateTime.Now}\t[Get]EnqueueWalletStatus");

        qWalletResult.Enqueue(json);
    }

    private void ProcessWalletStatus()
    {
        logger.LogDebug($"{DateTime.Now}\t Start ProcessWalletStatus");

        while (qWalletResult.TryDequeue(out var json))
        {
            // JSON 처리 로직 구현
            logger.LogDebug($"{DateTime.Now}\tProcessing wallet status");
        }
        logger.LogDebug($"{DateTime.Now}\t Finish ProcessWalletStatus");
    }
}