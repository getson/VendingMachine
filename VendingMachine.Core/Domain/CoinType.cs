namespace VendingMachine.Core.Domain
{
    public enum CoinType
    {
        TenCent = 10,

        TwentyCent = 20,

        HalfEuro = 50,

        OneEuro = 100
    }

    public class Coin
    {
        public Coin(int denomination, int count)
        {
            Denomination = denomination;
            Count = count;
        }

        public int Denomination { get; set; }
        public int Count { get; set; }
    }
}