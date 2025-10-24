using SmugglerCoin.Models;

namespace SmugglerCoin.UpbitModels.Models;

public class UpbitManager   // singleton 보장은...?
{
    // CurrentStatus Dictionary
    public Dictionary<string, int> CurrentStatus;

    private const float Loop_Seconds = 10f;     //1ms 단위

    private Smuggler PC;

    public UpbitManager()
    {
        InitManager();
    }

    public UpbitManager(Smuggler pc) : base()
    {
        this.PC = pc;

        InitManager();
    }

    private void InitManager()
    {
        CurrentStatus = new Dictionary<string, int>();

        if (PC == null) PC = new Smuggler();
    }
}