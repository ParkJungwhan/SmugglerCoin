using Microsoft.Extensions.Logging;
using Quartz;
using SmugglerCoin.Jobs;
using SmugglerCoin.Models;

namespace SmugglerCoin.UpbitModels.Jobs;

public class Job_Init : BaseJob
{
    public Job_Init(IAPICall apicaller, ILogger<Job_Init> _logger) : base(apicaller, _logger)
    {
    }

    public override async Task Execute(IJobExecutionContext context)
    {
        var initjob = Apicaller.GetCallAPI("market/all", null);
        var result = initjob.Result;
        //initjob.Start();

        SmuggerLog.LogInformation($"{DateTime.Now}\t Job_Init Execute() : {result}");

        //Console.WriteLine($"{DateTime.Now}\t Job_Init Execute() : {result}");

        // Upbit API 초기화 작업 수행
        //Apicaller.InitializeUpbitAPI();

        //return Task.CompletedTask;
    }
}