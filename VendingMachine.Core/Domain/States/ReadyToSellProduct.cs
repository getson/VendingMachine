using System.Collections.Generic;
using VendingMachine.Core.Domain;

namespace VendingMachine.Core
{
    public class ReadyToSellProduct : State
    {
        public ReadyToSellProduct(State state)
            : this(state.Wallet, state.Inventory, state.PricesProvider, state.VendingMachine)
        {
        }

        public ReadyToSellProduct(
            Wallet wallet, Inventory inventory,
            PricesProvider pricesProvider, VMachine machine
         )
        {
            InsertedCoins = new List<Coin>();

            Wallet = wallet;
            Inventory = inventory;
            PricesProvider = pricesProvider;
            VendingMachine = machine;
        }

        public override void CancelOrder()
        {
            // nothing to cancel
        }

        public override void InsertCoins(IEnumerable<Coin> coins)
        {
        }

        public override void ProcessOrder(IList<CoinWithQuantity> coinsToReturn)
        {
        }

        public override void SelectProduct(Product product)
        {
            if (Inventory.GetStock(product) < 1)
            {
                throw new ProductNotAvailableException($"{product} is not available!");
            }

            SelectedProduct = product;
            VendingMachine.State = new ReadyToAcceptCoins(this);
        }
    }
}