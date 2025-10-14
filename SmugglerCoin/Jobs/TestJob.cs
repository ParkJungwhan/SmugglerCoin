using System.Diagnostics;
using Quartz;

namespace SmugglerCoin.Jobs;

public class TestJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"{DateTime.Now} \t {context.Trigger.Key}");
        Debug.WriteLine($"{DateTime.Now} \t {context.Trigger.Key}");

        return Task.CompletedTask;
    }
}