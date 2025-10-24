using Microsoft.Extensions.Logging;
using Quartz;
using SmugglerCoin.Jobs;
using SmugglerCoin.Models;

namespace SmugglerCoin.UpbitModels.Jobs;

public class Job_APIKeys : BaseJob
{
    private const string ApiPath = "api_keys";

    public Job_APIKeys(IAPICall apicaller, ILogger<Job_APIKeys> logger) : base(apicaller, logger)
    {
    }

    public override async Task Execute(IJobExecutionContext context)
    {
        SmuggerLog.LogInformation($"{DateTime.Now}\tJob_APIKeys Execute() 시작");

        try
        {
            var response = await Apicaller.GetCallAPI(ApiPath, null);
            SmuggerLog.LogInformation($"{DateTime.Now}\tJob_APIKeys Execute() 응답: {response}");
        }
        catch (Exception ex)
        {
            SmuggerLog.LogError(ex, $"{DateTime.Now}\tJob_APIKeys Execute() 실패");
            throw;
        }
    }
}