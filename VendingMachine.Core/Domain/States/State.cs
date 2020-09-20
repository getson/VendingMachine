using System.Collections.Generic;
using VendingMachine.Core.Domain;

namespace VendingMachine.Core
{
    public abstract class State
    {
        public VMachine VendingMachine { get; set; }
        public Wallet Wallet { get; set; }
        public Inventory Inventory { get; set; }
        public PricesProvider PricesProvider { get; set; }

        public Product SelectedProduct { get; set; }
        public List<Coin> InsertedCoins { get; set; }

        public abstract void InsertCoins(IEnumerable<Coin> coins);

        public abstract void SelectProduct(Product product);

        public abstract void ProcessOrder(IList<CoinWithQuantity> coinsToReturn);

        public abstract void CancelOrder();
    }
}