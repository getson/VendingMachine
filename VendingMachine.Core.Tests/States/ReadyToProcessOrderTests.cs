using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VendingMachine.Core.Domain;
using VendingMachine.Core.Domain.Services;
using Xunit;

namespace VendingMachine.Core.Tests
{
    public class ReadyToProcessOrderTests
    {
        private Wallet _wallet = new Wallet(new Dictionary<Coin, int>
        {
             {Coin.TenCent,10},
             {Coin.TwentyCent,10},
             {Coin.HalfEuro,10 },
             {Coin.OneEuro,10 }
       });
        private Inventory _inventory = new Inventory(new Dictionary<Product, int>
        {
            {Product.Espresso,10 },
            {Product.Tea,10 },
            {Product.ChickenSoup,10 },
        });
        private PricesProvider _pricesProvider = new PricesProvider(
            new Dictionary<Product, int>
            {
                {Product.Espresso,120 },
                {Product.Juice,150 },
                {Product.Tea,100 },
            });
        private IChangeCalculator _changeCalculator = new ChangeCalculator();

        private ReadyToAcceptCoins CreateReadyToAcceptCoinsState()
        {
            var readyToSellState = new ReadyToSellProduct(
             _wallet,
             _inventory,
             _pricesProvider,
             new VMachine(_wallet, _inventory, _pricesProvider,
                 _changeCalculator
             )
         );
            return new ReadyToAcceptCoins(readyToSellState);

        }

        [Fact]
        public void CancelOrderSuccess()
        {
            var state = new ReadyToProcessOrder(CreateReadyToAcceptCoinsState());

            state.CancelOrder();
            Assert.True(state.VendingMachine.State is ReadyToSellProduct);
        }

        [Fact]
        public void ProcessOrderSuccess()
        {
            var state = new ReadyToProcessOrder(CreateReadyToAcceptCoinsState())
            {
                SelectedProduct = Product.Espresso,
                InsertedCoins = new List<Coin>
                {
                    Coin.OneEuro,
                    Coin.HalfEuro
                }
            };

            var coinsToReturn = new List<CoinWithQuantity>()
            {
                new CoinWithQuantity(Coin.TwentyCent,1),
                new CoinWithQuantity(Coin.TenCent,1)
            };

            var oneEuroQuantityBefore = state.Wallet.GetQuantity(Coin.OneEuro);
            var halfEuroQuantityBefore = state.Wallet.GetQuantity(Coin.HalfEuro);
            var twentyCentQuantityBefore = state.Wallet.GetQuantity(Coin.TwentyCent);
            var tenCentQuantityBefore = state.Wallet.GetQuantity(Coin.TenCent);


            var espressoStockBefore = state.Inventory.GetStock(Product.Espresso);

            state.ProcessOrder(coinsToReturn);

            var oneEuroQuantityAfter = state.Wallet.GetQuantity(Coin.OneEuro);
            var halfEuroQuantityAfter = state.Wallet.GetQuantity(Coin.HalfEuro);
            var twentyQuantityAfter = state.Wallet.GetQuantity(Coin.TwentyCent);
            var tenCentQuantityAfter = state.Wallet.GetQuantity(Coin.TenCent);

            var espressoStockAfter = state.Inventory.GetStock(Product.Espresso);




            Assert.True(state.VendingMachine.State.InsertedCoins.Count == 0); // insertedCoins is cleared
            Assert.True(oneEuroQuantityAfter == oneEuroQuantityBefore + 1);
            Assert.True(halfEuroQuantityAfter == halfEuroQuantityBefore + 1);

            Assert.True(tenCentQuantityAfter == tenCentQuantityBefore - 1);
            Assert.True(twentyQuantityAfter == twentyCentQuantityBefore - 1);

            Assert.True(espressoStockAfter == espressoStockBefore - 1);

            Assert.True(state.VendingMachine.State is ReadyToSellProduct);
        }

    }
}
