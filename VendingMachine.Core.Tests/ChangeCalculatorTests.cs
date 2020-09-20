using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Core.Domain;
using VendingMachine.Core.Domain.Services;
using Xunit;

namespace VendingMachine.Core.Tests
{
    public class ChangeCalculatorTests
    {
        IChangeCalculator _changeCalculator = new ChangeCalculator();

        [Fact]
        public void MinimumCoins()
        {
            var coinsWithQuantity = new List<CoinWithQuantity>
            {
                new CoinWithQuantity(Coin.OneEuro,3),
                new CoinWithQuantity(Coin.HalfEuro,5),
                new CoinWithQuantity(Coin.TwentyCent,5),
                new CoinWithQuantity(Coin.TenCent,10)
            };
            var changeCoins = _changeCalculator.CalculateMinimum(coinsWithQuantity, 370)
                 .ToDictionary(grp => grp.Denomination, v => v.Quantity);
            //100 100 100 50 20
            Assert.Equal(3, changeCoins.Count);
            Assert.Equal(3, changeCoins[100]);
            Assert.Equal(1, changeCoins[50]);
            Assert.Equal(1, changeCoins[20]);

        }
        [Fact]
        public void NotAbleToProduceChange()
        {
            var coinsWithQuantity = new List<CoinWithQuantity>
            {
                new CoinWithQuantity(Coin.TwentyCent,3),
                new CoinWithQuantity(Coin.HalfEuro,5)
            };
            var changeCoins = _changeCalculator.CalculateMinimum(coinsWithQuantity, 30);

            Assert.Null(changeCoins);
        }
        [Fact]
        public void MinimumCoinsIsTheSameCoinTwoTimes()
        {
            var coinsWithQuantity = new List<CoinWithQuantity>
            {
                new CoinWithQuantity(Coin.HalfEuro,3),
                new CoinWithQuantity(Coin.TwentyCent,2),
                new CoinWithQuantity(Coin.TenCent,2),
            };
            var changeCoins = _changeCalculator.CalculateMinimum(coinsWithQuantity, 40);

            Assert.Equal(1, changeCoins.Count);
            Assert.Equal(2, changeCoins[0].Quantity);
            Assert.Equal(20, changeCoins[0].Denomination);
        }
    }
}
