using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VendinMachine.Core
{
    public enum Coin
    {
        TenCent = 10,

        TwentyCent = 20,

        HalfEuro = 50,

        OneEuro = 100
    }
    public enum Product
    {
        Tea = 130,

        Esspresso = 180,

        Juice = 180,

        ChickenSoup = 180
    }
    public class VendingMachine
    {
        public VendingMachine(
            Dictionary<Coin, int> wallet,
            Dictionary<Product, int> stock
        )
        {
            State = new ReadyToAcceptCoins
             (
                wallet,
                stock
            );
        }

        public State State { get; set; }
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
    }
    public abstract class State
    {
        protected Dictionary<Coin, int> _wallet;
        protected Dictionary<Product, int> _stock;
        public VendingMachine VendingMachine { get; set; }
        public Product SelectedProduct { get; set; }
        public List<Coin> InsertedCoins { get; set; }
        public abstract void InsertCoins(IEnumerable<Coin> coins);
        public abstract void SelectProduct(Product product);
        public abstract void ProcessOrder();
        public abstract void CancelOrder();
    }
    public class ReadyToSelectProduct : State
    {
        public ReadyToSelectProduct(State state)
        {
            VendingMachine = state.VendingMachine;
            InsertedCoins = state.InsertedCoins;
            SelectedProduct = state.SelectedProduct;
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
            throw new InvalidOperationException("Please select the product!");
        }

        public override void SelectProduct(Product product)
        {
            SelectedProduct = product;
            VendingMachine.State = new ReadyToAcceptCoins(this);
        }
    }
    public class ReadyToAcceptCoins : State
    {
        public ReadyToAcceptCoins(
            Dictionary<Coin, int> initialWallet,
            Dictionary<Product, int> initialStock)
        {
            _wallet = initialWallet;
            _stock = initialStock;
        }

        public ReadyToAcceptCoins(State state)
        {
            VendingMachine = state.VendingMachine;
            InsertedCoins = state.InsertedCoins;
            SelectedProduct = state.SelectedProduct;
        }

        public override void CancelOrder()
        {
            VendingMachine.State = new ReadyToSelectProduct(this);
        }

        public override void InsertCoins(IEnumerable<Coin> coins)
        {
            InsertedCoins = coins.ToList();
            VendingMachine.State = new ReadyToSelectProduct(this);
        }

        public override void ProcessOrder()
        {
            throw new InvalidOperationException("Please insert your coins!");
        }

        public override void SelectProduct(Product product)
        {
            throw new InvalidOperationException("Please insert your coins!");
        }
    }
    public class ReadyToProcessOrder : State
    {
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
