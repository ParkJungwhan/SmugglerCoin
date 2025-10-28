namespace SmugglerCoin.Models;

public class CoroutineManager
{
    private readonly List<Task> _runningTasks = new();
    private readonly CancellationTokenSource _cts = new();

    public void Start(Func<CancellationToken, Task> coroutine)
    {
        var task = Task.Run(() => coroutine(_cts.Token), _cts.Token);
        _runningTasks.Add(task);
    }

    public async Task RunAllAsync()
    {
        try
        {
            await Task.WhenAll(_runningTasks);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Coroutine execution cancelled.");
        }
    }

    public void StopAll()
    {
        _cts.Cancel();
    }
}

public static class Wait
{
    // Unity의 yield return new WaitForSeconds() 와 유사
    public static async Task ForSeconds(float seconds, CancellationToken token)
    {
        await Task.Delay(TimeSpan.FromSeconds(seconds), token);
    }

    // Unity의 yield return null 과 유사 (다음 프레임까지)
    public static async Task NextFrame(CancellationToken token)
    {
        await Task.Yield(); // 즉시 다음 스케줄링 포인트로 넘김
    }
}