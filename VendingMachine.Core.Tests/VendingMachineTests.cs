using System.Collections.Generic;
using VendingMachine.Core.Domain;
using VendingMachine.Core.Domain.Services;
using Xunit;

namespace VendingMachine.Core.Tests
{
    public class VendingMachineTests
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

        [Fact]
        public void SelectProductSuccess()
        {
            var vmachine = new VMachine(_wallet, _inventory, _pricesProvider, _changeCalculator);

            vmachine.SelectProduct(Product.Espresso);

            Assert.True(vmachine.State is ReadyToAcceptCoins);
        }

        [Fact]
        public void InsertCoinsSuccess()
        {
            var vmachine = new VMachine(_wallet, _inventory, _pricesProvider, _changeCalculator);

            vmachine.SelectProduct(Product.Espresso);
            vmachine.InsertCoins(new List<Coin>
                {
                    Coin.HalfEuro, Coin.OneEuro
                }
            );

            Assert.True(vmachine.State is ReadyToProcessOrder);
        }

        [Fact]
        public void CancelOrderSuccess()
        {
            var vmachine = new VMachine(_wallet, _inventory, _pricesProvider, _changeCalculator);

            vmachine.SelectProduct(Product.Espresso);
            vmachine.InsertCoins(new List<Coin>
                {
                    Coin.HalfEuro, Coin.OneEuro
                }
            );
            vmachine.CancelOrder();

            Assert.True(vmachine.State is ReadyToSellProduct);
        }

        [Fact]
        public void ProcessOrderWithChangeSuccess()
        {
            var vmachine = new VMachine(_wallet, _inventory, _pricesProvider, _changeCalculator);

            vmachine.SelectProduct(Product.Espresso);
            vmachine.InsertCoins(new List<Coin>
                {
                    Coin.HalfEuro, Coin.OneEuro
                }
            );

            var coinsToReturn = vmachine.ProcessOrder(false);

            Assert.True(coinsToReturn.Count == 2);

            Assert.True(vmachine.State is ReadyToSellProduct);
        }

        [Fact]
        public void ProcessOrderWithChangeFail()
        {
            var wallet = new Wallet(new Dictionary<Coin, int>
            {
                 {Coin.TenCent,0},
                 {Coin.TwentyCent,0},
                 {Coin.HalfEuro,10 },
                 {Coin.OneEuro,10 }
            });

            var vmachine = new VMachine(wallet, _inventory, _pricesProvider, _changeCalculator);

            vmachine.SelectProduct(Product.Espresso);
            vmachine.InsertCoins(new List<Coin>
                {
                    Coin.HalfEuro, Coin.OneEuro
                }
            );

            Assert.Throws<NotSufficientChangeException>(
                () => vmachine.ProcessOrder(false)
            );

            Assert.True(vmachine.State is ReadyToProcessOrder);
        }

        [Fact]
        public void ProcessOrderWithNoChangeBackSuccess()
        {
            var wallet = new Wallet(new Dictionary<Coin, int>
            {
                 {Coin.TenCent,0},
                 {Coin.TwentyCent,0},
                 {Coin.HalfEuro,10 },
                 {Coin.OneEuro,10 }
            });

            var vmachine = new VMachine(wallet, _inventory, _pricesProvider, _changeCalculator);

            vmachine.SelectProduct(Product.Espresso);
            vmachine.InsertCoins(new List<Coin>
                {
                    Coin.HalfEuro, Coin.OneEuro
                }
            );

            var coinsToReturn = vmachine.ProcessOrder(true);

            Assert.True(vmachine.State is ReadyToSellProduct);
        }
    }
}