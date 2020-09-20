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

        public override void InsertCoins(IEnumerable<CoinType> coins)
        {
            InsertedCoins = coins.ToList();

            var productCost = PricesProvider.GetPrice(SelectedProduct);
            var diff = productCost - InsertedCoins.Sum(x => (int)x);

            if (diff > 0)
            {
                throw new NotFullyPaidException("The selected product is not fully paid, Please insert the remaining amount!", diff);
            }
            VendingMachine.State = new ReadyToProcessOrder(this);
        }

        public override void ProcessOrder(IList<Coin> coinsToReturn)
        {
        }

        public override void SelectProduct(Product product)
        {
        }
    }
}