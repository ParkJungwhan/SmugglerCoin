using Quartz;

namespace SmugglerCoin.Jobs;

public class DefaultJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        // job으로 등록된 스케줄러가 호출될때마다 동작하는부분
        Console.WriteLine($"{DateTime.Now}\t DefaultJob Execute()");

        return Task.CompletedTask;
    }
}