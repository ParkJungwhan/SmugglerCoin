using Microsoft.Extensions.Logging;
using Quartz;
using SmugglerCoin.Jobs;
using SmugglerCoin.Models;
using SmugglerCoin.UpbitModels.Models;

namespace SmugglerCoin.UpbitModels.Jobs;

public class Job_StatusWallet : BaseJob
{
    private const string ApiPath = "status/wallet";

    public Job_StatusWallet(IAPICall apicaller, ILogger<Job_StatusWallet> logger, UpbitManager manager) :
        base(apicaller, logger, manager)
    {
        //MaxRetryCount = 30 - 1;   // 초당 최대 30회 가능
    }

    public override async Task Execute(IJobExecutionContext context)
    {
        SmuggerLog.LogInformation($"{DateTime.Now}\t{context.Trigger.Key} || Job_StatusWallet Execute() 시작");

        try
        {
            var response = await Apicaller.GetCallAPI(ApiPath, null, true);
            if (string.IsNullOrEmpty(response))
            {
                SmuggerLog.LogInformation($"{DateTime.Now}\tJob_StatusWallet Execute() 응답: {response}");
                return;
            }

            Manager.EnqueueWalletStatus(response);
        }
        catch (Exception ex)
        {
            SmuggerLog.LogError(ex, $"{DateTime.Now}\tJob_StatusWallet Execute() 실패");
            throw;
        }
    }
}