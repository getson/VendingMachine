using System.Collections.Generic;
using VendingMachine.Core.Domain;

namespace VendingMachine.Core
{
    public class Wallet
    {
        private readonly Dictionary<Coin, int> _coins;

        public Wallet(Dictionary<Coin, int> coins)
        {
            _coins = coins;
        }

        public void Add(Coin coin, int count)
        {
            _coins.TryGetValue(coin, out var actualCount);

            _coins[coin] = actualCount + count;
        }

        public void Deduct(Coin coin, int count = 1)
        {
            _coins.TryGetValue(coin, out var actualCount);

            _coins[coin] = actualCount - count;
        }
    }
}