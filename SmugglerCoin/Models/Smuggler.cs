using SmugglerCoin.BithumbModels;

namespace SmugglerCoin.Models;

public class Smuggler
{
    // 얘가 유저이며 얘가 여러개의 코인을 가질수 있다.
    public Dictionary<string, Coin> coins { get; set; }

    public UserToken APITokens { get; set; }

    public Smuggler()
    {
        bool flag;
        if (flag = true) // flag에 true 대입 → 조건식은 true로 평가됨
        {
            Console.WriteLine("조건이 참입니다.");
        }
    }

    public void InitCoins()
    {
        // 여기서 코인의 모든 목록을 받아와서 초기화 한다

        coins = new Dictionary<string, Coin>();
    }
}