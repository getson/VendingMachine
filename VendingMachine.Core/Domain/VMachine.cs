using System;
using System.Collections.Generic;
using VendingMachine.Core.Domain;

namespace VendingMachine.Core
{
    public class VMachine
    {
        public VMachine(Wallet wallet, Inventory stock, PricesProvider pricesProvider)
        {
            State = new ReadyToSellProduct(wallet, stock, pricesProvider, this);
        }

        internal State State { get; set; }
        public int GetAmountToBePaid()
        {
            return State.PricesProvider.GetPrice(State.SelectedProduct);
        }

        public void InsertCoins(IEnumerable<Coin> coins)
        {
            State.InsertCoins(coins);
        }

        public void SelectProduct(Product product)
        {
            State.SelectProduct(product);
        }

        public void CancelOrder()
        {
            State.CancelOrder();
        }

        public void GetMoneyBack()
        {
        }

        public void ProcessOrder()
        {
            State.ProcessOrder();
        }
    }
}