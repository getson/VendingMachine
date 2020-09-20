using System.Collections.Generic;
using System.Linq;
using VendingMachine.Core.Domain;

namespace VendingMachine.Core
{
    internal class ReadyToAcceptCoins : State
    {
        public ReadyToAcceptCoins(State state)
        {
            VendingMachine = state.VendingMachine;
            InsertedCoins = state.InsertedCoins;
            SelectedProduct = state.SelectedProduct;

            Wallet = state.Wallet;
            Inventory = state.Inventory;
            PricesProvider = state.PricesProvider;
        }

        public override void CancelOrder()
        {
            VendingMachine.State = new ReadyToSellProduct(this);
        }

        public override void InsertCoins(IEnumerable<Coin> coins)
        {
            InsertedCoins.AddRange(coins);

            var productCost = PricesProvider.GetPrice(SelectedProduct);
            var diff = productCost - InsertedCoins.Sum(x => (int)x);

            if (diff > 0)
            {
                throw new NotSufficentAmountException("Insufficient amount! /n Insert remaining amount", diff);
            }
            VendingMachine.State = new ReadyToProcessOrder(this);
        }

        public override void ProcessOrder(IList<CoinWithQuantity> coinsToReturn)
        {
        }

        public override void SelectProduct(Product product)
        {
        }
    }
}