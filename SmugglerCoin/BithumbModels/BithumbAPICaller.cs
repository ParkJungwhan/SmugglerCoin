using SmugglerCoin.Models;

namespace SmugglerCoin.BithumbModels;

public class BithumbAPICaller : BaseAPICall
{
    public override Task GetCallAPI(string method, Dictionary<string, string>? dicParams)
    {
        // implementation needed

        return Task.CompletedTask;
    }
}
