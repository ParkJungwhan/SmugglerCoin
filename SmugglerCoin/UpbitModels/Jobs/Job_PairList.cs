using System.Text.Json;
using Microsoft.Extensions.Logging;
using Quartz;
using SmugglerCoin.Jobs;
using SmugglerCoin.Models;
using SmugglerCoin.UpbitModels.Models;

namespace SmugglerCoin.UpbitModels.Jobs;

public class Job_PairList : BaseJob
{
    private const string ApiPath = "market/all";
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public Job_PairList(IAPICall apicaller, ILogger<Job_PairList> logger, UpbitManager manager)
        : base(apicaller, logger, manager)
    {
    }

    public override async Task Execute(IJobExecutionContext context)
    {
        SmuggerLog.LogInformation($"{DateTime.Now}\t{context.Trigger.Key} || Job_PairList Execute() 시작");

        try
        {
            var response = await Apicaller.GetCallAPI($"{ApiPath}?isDetails=true", null);
            if (string.IsNullOrWhiteSpace(response))
            {
                SmuggerLog.LogWarning($"{DateTime.Now}\tJob_PairList 응답이 비어 있습니다.");
                return;
            }

            List<MarketAllModel>? markets;
            try
            {
                markets = JsonSerializer.Deserialize<List<MarketAllModel>>(response, JsonOptions);
            }
            catch (JsonException ex)
            {
                SmuggerLog.LogError(ex, $"{DateTime.Now}\tJob_PairList 응답 파싱 실패");
                return;
            }

            if (markets is null || markets.Count == 0)
            {
                SmuggerLog.LogWarning($"{DateTime.Now}\tJob_PairList 파싱 결과가 없습니다.");
                return;
            }

            Manager.UpdateMarketPairs(markets);
            SmuggerLog.LogInformation($"{DateTime.Now}\tJob_PairList 완료: 총 {markets.Count}개 마켓 갱신");
        }
        catch (Exception ex)
        {
            SmuggerLog.LogError(ex, $"{DateTime.Now}\tJob_PairList Execute() 실패");
            throw;
        }
    }
}
