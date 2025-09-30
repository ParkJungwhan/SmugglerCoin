namespace SmugglerCoin.Models;

public abstract class BaseAPICall : IAPICall
{
    protected HttpClient Client;
    protected string BASE_URL;

    public virtual bool SetInitAPI()
    {
        Client = new HttpClient();
        Client.BaseAddress = new Uri(BASE_URL);

        return true;
    }
}