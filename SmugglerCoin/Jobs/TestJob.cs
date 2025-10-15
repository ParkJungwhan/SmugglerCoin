using System.Diagnostics;
using Quartz;
using SmugglerCoin.Models;

namespace SmugglerCoin.Jobs;

public class TestJob : IJob
{
    private IAPICall _api;

    public TestJob(IAPICall api)
    {
        _api = api;
    }

    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"{DateTime.Now} \t {context.Trigger.Key}");
        Debug.WriteLine($"{DateTime.Now} \t {context.Trigger.Key}");

        return Task.CompletedTask;
    }
}