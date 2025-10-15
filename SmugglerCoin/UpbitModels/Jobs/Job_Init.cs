using Quartz;
using SmugglerCoin.Jobs;
using SmugglerCoin.Models;

namespace SmugglerCoin.UpbitModels.Jobs;

public class Job_Init : BaseJob
{
    public Job_Init(IAPICall apicaller) : base(apicaller)
    {
    }

    public override Task Execute(IJobExecutionContext context)
    {
        Apicaller.GetCallAPI("market/all", null);

        Console.WriteLine($"{DateTime.Now}\t Job_Init Execute()");
        // Upbit API 초기화 작업 수행
        //Apicaller.InitializeUpbitAPI();
        return Task.CompletedTask;
    }
}