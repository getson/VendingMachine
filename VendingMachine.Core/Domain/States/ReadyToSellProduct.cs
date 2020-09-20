using System.Collections.Generic;
using VendingMachine.Core.Domain;

namespace VendingMachine.Core
{
    internal class ReadyToSellProduct : State
    {
        public ReadyToSellProduct(State state)
        {
            VendingMachine = state.VendingMachine;
            InsertedCoins = state.InsertedCoins;
            SelectedProduct = state.SelectedProduct;

            Wallet = state.Wallet;
            Inventory = state.Inventory;
            PricesProvider = state.PricesProvider;
        }

        public ReadyToSellProduct(
            Wallet wallet, Inventory inventory,
            PricesProvider pricesProvider, VMachine machine
         )
        {
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

        public override void ProcessOrder()
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