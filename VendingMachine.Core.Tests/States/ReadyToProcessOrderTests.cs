using System.Collections.Generic;
using VendingMachine.Core.Domain;
using VendingMachine.Core.Domain.Services;
using Xunit;

namespace VendingMachine.Core.Tests
{
    public class ReadyToProcessOrderTests
    {
        private Wallet _wallet = new Wallet(new Dictionary<Coin, int>
        {
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
                {Product.Espresso,100 },
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
    }
}
