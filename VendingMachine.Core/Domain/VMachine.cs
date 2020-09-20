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

        public void InsertCoins(IEnumerable<Coin> coins)
        {
            State.InsertCoins(coins);
        }

        public void SelectProduct(Product product)
        {
            State.SelectProduct(product);
        }

        public IReadOnlyCollection<Coin> CancelOrder()
        {
            var insertedCoins = State.InsertedCoins;

            State.CancelOrder();

            return insertedCoins.AsReadOnly();
        }

        public Product GetSelectedProduct()
        {
            return State.SelectedProduct;
        }
        public IReadOnlyCollection<Coin> ProcessOrder(bool ignoreChange)
        {

            var coinWithQuantitiesChange = GetChange();

            if (!ignoreChange && coinWithQuantitiesChange == null)
            {
                throw new NotSufficientChangeException(
                    "Not able to return change"
                );
            }

            State.ProcessOrder(coinWithQuantitiesChange);

            var coinsToReturn = new List<Coin>();

            foreach (var coin in coinWithQuantitiesChange)
            {
                coinsToReturn.AddRange(
                    Enumerable.Repeat((Coin)coin.Denomination, coin.Quantity)
                );
            }

            return coinsToReturn.AsReadOnly();
        }

        public IReadOnlyDictionary<Product, int> GetProductsWithPrices()
        {
            return State.PricesProvider.GetAll();
        }
       
        private IList<CoinWithQuantity> GetChange()
        {
            var insertedAmount = State.InsertedCoins.Sum(x => (int)x);
            var change = insertedAmount - State.PricesProvider.GetPrice(State.SelectedProduct);

            var insertedCoinsWithQuantity = State.InsertedCoins
                .GroupBy(x => x)
                .ToDictionary(grp => grp.Key, v => v.Count());


            var coinsWithQuantity = new Dictionary<Coin, int>(
                    State.Wallet.GetAll()
                );

            foreach (var c in insertedCoinsWithQuantity)
            {
                coinsWithQuantity.TryGetValue(c.Key, out var quantity);

                coinsWithQuantity[c.Key] = quantity + c.Value;
            }


            var coinsToReturn = _changeCalculator.CalculateMinimum(
                coinsWithQuantity.Select(x => new CoinWithQuantity(x.Key, x.Value))
                .OrderByDescending(x => x.Denomination).ToList(),
                change
            );
            return coinsToReturn;
        }
    }
}