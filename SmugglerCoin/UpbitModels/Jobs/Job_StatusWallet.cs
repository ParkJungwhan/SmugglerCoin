using Microsoft.Extensions.Logging;
using Quartz;
using SmugglerCoin.Jobs;
using SmugglerCoin.Models;

namespace SmugglerCoin.UpbitModels.Jobs;

public class Job_StatusWallet : BaseJob
{
    private const string ApiPath = "status/wallet";

    public Job_StatusWallet(IAPICall apicaller, ILogger<Job_StatusWallet> logger) : base(apicaller, logger)
    {
    }

    public override async Task Execute(IJobExecutionContext context)
    {
        SmuggerLog.LogInformation($"{DateTime.Now}\tJob_StatusWallet Execute() 시작");

        try
        {
            var response = await Apicaller.GetCallAPI(ApiPath, null);
            SmuggerLog.LogInformation($"{DateTime.Now}\tJob_StatusWallet Execute() 응답: {response}");
        }
        catch (Exception ex)
        {
            SmuggerLog.LogError(ex, $"{DateTime.Now}\tJob_StatusWallet Execute() 실패");
            throw;
        }
    }
}