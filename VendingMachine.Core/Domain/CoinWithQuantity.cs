namespace VendingMachine.Core.Domain
{
    public class CoinWithQuantity
    {
        public CoinWithQuantity(Coin coin, int quantity)
        {
            Denomination = (int)coin;
            Quantity = quantity;
        }

        public int Denomination { get; set; }
        public int Quantity { get; set; }
    }
}