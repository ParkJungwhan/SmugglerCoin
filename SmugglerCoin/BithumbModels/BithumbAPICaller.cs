using SmugglerCoin.Models;

namespace SmugglerCoin.BithumbModels;

public class BithumbAPICaller : BaseAPICall
{
    public override Task<string> GetCallAPI(string method, Dictionary<string, string>? dicParams, bool bAuth)
    {
        // implementation needed

        return Task.FromResult("");
    }
}