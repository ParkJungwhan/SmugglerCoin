using Microsoft.Extensions.Logging;
using Quartz;
using SmugglerCoin.Jobs;
using SmugglerCoin.Models;

namespace SmugglerCoin.UpbitModels.Jobs;

public class PairList : BaseJob
{
    private Dictionary<string, bool> DicParam;

    public PairList(IAPICall apicaller, ILogger<PairList> _logger) : base(apicaller, _logger)
    {
        DicParam = new Dictionary<string, bool>(1);
        DicParam.Add("is_details", true);
    }

    public override async Task Execute(IJobExecutionContext context)
    {
        //        DicParam
        var initjob = Apicaller.GetCallAPI("market/all", null);
        var result = initjob.Result;

        // Upbit - 페어 목록 조회 : 결과값 반영
        SmuggerLog.LogInformation($"{DateTime.Now}\t Job_Init Execute() : {result}");
    }
}