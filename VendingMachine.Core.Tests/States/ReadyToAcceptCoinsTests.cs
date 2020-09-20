using System.Collections.Generic;
using Xunit;
using VendingMachine.Core.Domain;
using VendingMachine.Core.Domain.Services;
using System.Linq;

namespace VendingMachine.Core.Tests
{
    public class ReadyToAcceptCoinsTests
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

        private ReadyToSellProduct CreateReadyToSellProductState()
        {
            return new ReadyToSellProduct(
             _wallet,
             _inventory,
             _pricesProvider,
             new VMachine(_wallet, _inventory, _pricesProvider,
                 _changeCalculator
             )
         );
        }

        [Fact]
        public void InsertCoinsAndChangeStateSuccess()
        {

            var state = new ReadyToAcceptCoins(CreateReadyToSellProductState());

            state.InsertCoins(new List<Coin>
                {
                    Coin.HalfEuro,
                    Coin.OneEuro
                }
            );

            var insertedCoins = state.InsertedCoins.ToList();

            Assert.True(insertedCoins.Count == 2);
            Assert.Equal(Coin.HalfEuro, insertedCoins[0]);
            Assert.Equal(Coin.OneEuro, insertedCoins[1]);

            Assert.True(state.VendingMachine.State is ReadyToProcessOrder);
        }

        [Fact]
        public void SelectNonExistentProductFail()
        {


            var readyToSellState = CreateReadyToSellProductState();
            readyToSellState.SelectProduct(Product.Espresso);

            var state = new ReadyToAcceptCoins(readyToSellState);

            Assert.Throws<NotSufficientAmountException>(
                () => state.InsertCoins(new List<Coin> { Coin.HalfEuro })
            );

            Assert.True(state.VendingMachine.State is ReadyToAcceptCoins);
        }
    }
}
