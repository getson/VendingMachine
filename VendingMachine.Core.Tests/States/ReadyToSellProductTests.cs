using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using VendingMachine.Core.Domain;
using VendingMachine.Core.Domain.Services;

namespace VendingMachine.Core.Tests
{
    public class ReadyToSellProductTests
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
            });
        private IChangeCalculator _changeCalculator = new ChangeCalculator();

        [Fact]
        public void SelectProductAndChangeStateSuccess()
        {

            var state = new ReadyToSellProduct(
                _wallet,
                _inventory,
                _pricesProvider,
                new VMachine(_wallet, _inventory, _pricesProvider,
                    _changeCalculator
                )
            );

            state.SelectProduct(Product.Espresso);

            Assert.Equal(Product.Espresso,
                    state.SelectedProduct
                );
            Assert.True(state.VendingMachine.State is ReadyToAcceptCoins);
        }

        [Fact]
        public void SelectNonExistentProductFail()
        {


            var state = new ReadyToSellProduct(
                _wallet,
                _inventory,
                _pricesProvider,
                new VMachine(_wallet, _inventory, _pricesProvider,
                    _changeCalculator
                )
            );


            Assert.Throws<ProductNotAvailableException>(
                () => state.SelectProduct(Product.Juice)
            );
            Assert.True(state.VendingMachine.State is ReadyToSellProduct);
        }
    }
}
