using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using VendingMachine.Core.Domain;
using VendingMachine.Core.Domain.Services;

namespace VendingMachine.Core
{
    public class VMachine
    {
        private readonly IChangeCalculator _changeCalculator;

        public VMachine(
            Wallet wallet, Inventory stock,
            PricesProvider pricesProvider, IChangeCalculator changeCalculator
        )
        {
            State = new ReadyToSellProduct(wallet, stock, pricesProvider, this);
            _changeCalculator = changeCalculator;
        }

        internal State State { get; set; }

        public int GetAmountToBePaid()
        {
            return State.PricesProvider.GetPrice(State.SelectedProduct);
        }

        public void InsertCoins(IEnumerable<CoinType> coins)
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
            var insertedAmount = State.InsertedCoins.Sum(x => (int)x);
            var change = insertedAmount - State.PricesProvider.GetPrice(State.SelectedProduct);

            var insertedCoinsWithQuantity = State.InsertedCoins
                .GroupBy(x => x)
                .ToDictionary(grp => grp.Key, v => v.Count());


            var coinsWithQuantity = new Dictionary<CoinType, int>(
                    State.Wallet.GetAll()
                );

            foreach (var c in insertedCoinsWithQuantity)
            {
                coinsWithQuantity.TryGetValue(c.Key, out var quantity);

                coinsWithQuantity[c.Key] = quantity + c.Value;
            }


            var coinsToReturn = _changeCalculator.CalculateMinimum(
                coinsWithQuantity.Select(x => new Coin((int)x.Key, x.Value)).ToList(),
                change
            );
            if (coinsToReturn == null)
            {
                throw new NotSufficientChangeException("Not able to return change! Please, Insert exact change!");
            }
            State.ProcessOrder(coinsToReturn);
        }

        public ReadOnlyDictionary<Product, int> GetProductsWithPrices()
        {
            return State.PricesProvider.GetAll();
        }
    }
}