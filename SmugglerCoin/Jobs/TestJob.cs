using Quartz;

namespace SmugglerCoin.Jobs;

public class TestJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"{DateTime.Now}\t {context.FireInstanceId}");
        Console.WriteLine($"{DateTime.Now}\t TestJob Execute()");

        return Task.CompletedTask;
    }
}