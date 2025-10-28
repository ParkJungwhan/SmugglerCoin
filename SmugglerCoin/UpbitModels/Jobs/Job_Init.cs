using Microsoft.Extensions.Logging;
using Quartz;
using SmugglerCoin.Jobs;
using SmugglerCoin.Models;
using SmugglerCoin.UpbitModels.Models;

namespace SmugglerCoin.UpbitModels.Jobs;

public class Job_Init : BaseJob
{
    private const string ApiPath = "market/all";

    public Job_Init(IAPICall apicaller, ILogger<Job_Init> _logger, UpbitManager manager) : base(apicaller, _logger, manager)
    {
    }

    public override async Task Execute(IJobExecutionContext context)
    {
        var initjob = Apicaller.GetCallAPI(ApiPath, null);
        var result = initjob.Result;

        // Upbit - 페어 목록 조회 : 결과값 반영
        SmuggerLog.LogInformation($"{DateTime.Now}\t Job_Init Execute() : {result}");
    }
}