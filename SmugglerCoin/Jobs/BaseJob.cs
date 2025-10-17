using Microsoft.Extensions.Logging;
using Quartz;
using SmugglerCoin.Models;

namespace SmugglerCoin.Jobs;

public abstract class BaseJob : IJob
{
    public IAPICall Apicaller { get; }

    protected ILogger<BaseJob> SmuggerLog;

    public BaseJob(IAPICall apicaller, ILogger<BaseJob> _logger)
    {
        Apicaller = apicaller;
        SmuggerLog = _logger;
    }

    public virtual Task Execute(IJobExecutionContext context)
    {
        // job으로 등록된 스케줄러가 호출될때마다 동작하는부분
        //Console.WriteLine($"{DateTime.Now}\t DefaultJob Execute()");
        SmuggerLog.LogDebug($"{DateTime.Now}\t DefaultJob Execute()");

        return Task.CompletedTask;
    }
}