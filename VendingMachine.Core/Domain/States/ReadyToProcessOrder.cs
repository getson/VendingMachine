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
            VendingMachine.State = new ReadyToSellProduct(this);
        }

        public override void InsertCoins(IEnumerable<Coin> coins)
        {
        }

        public override void ProcessOrder(IList<CoinWithQuantity> coinsToReturn)
        {
            foreach (var coin in InsertedCoins)
            {
                Wallet.Add(coin, 1);
            }
            foreach (var coin in coinsToReturn)
            {
                Wallet.Deduct((Coin)coin.Denomination, coin.Quantity);
            }
            Inventory.Deduct(SelectedProduct);
            
            VendingMachine.State = new ReadyToSellProduct(this);
        }
        public override void SelectProduct(Product product)
        {
        }

    }
}