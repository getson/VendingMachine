using System.Collections.Generic;
using VendingMachine.Core.Domain;

namespace VendingMachine.Core
{
     class ReadyToProcessOrder : State
    {
        public ReadyToProcessOrder(State state)
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
            VendingMachine.State = new ReadyToAcceptCoins(this);
        }

        public override void InsertCoins(IEnumerable<Coin> coins)
        {
        }

        public override void ProcessOrder()
        {
            //TODO do the calculation
            VendingMachine.State = new ReadyToAcceptCoins(this);
        }

        public override void SelectProduct(Product product)
        {
        }
    }
}