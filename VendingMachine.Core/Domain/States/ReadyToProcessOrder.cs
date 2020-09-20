using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Core.Domain;

namespace VendingMachine.Core
{
    internal class ReadyToProcessOrder : State
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

        public override void InsertCoins(IEnumerable<CoinType> coins)
        {
        }

        public override void ProcessOrder(IList<Coin> coinsToReturn)
        {
            foreach (var coin in InsertedCoins)
            {
                Wallet.Add(coin, 1);
            }
            foreach (var coin in coinsToReturn)
            {
                Wallet.Deduct((CoinType)coin.Denomination, coin.Count);
            }
            Inventory.Deduct(SelectedProduct);

            VendingMachine.State = new ReadyToAcceptCoins(this);
        }
        public override void SelectProduct(Product product)
        {
        }

    }
}