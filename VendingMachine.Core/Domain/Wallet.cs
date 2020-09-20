using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using VendingMachine.Core.Domain;

namespace VendingMachine.Core
{
    public class Wallet
    {
        private readonly Dictionary<CoinType, int> _grouppedCoins;

        public Wallet(Dictionary<CoinType, int> coins)
        {
            _grouppedCoins = coins;
        }

        public void Add(CoinType coin, int quantity)
        {
            _grouppedCoins.TryGetValue(coin, out var actualQuantity);

            _grouppedCoins[coin] = actualQuantity + quantity;
        }

        public void Deduct(CoinType coin, int quantity = 1)
        {
            _grouppedCoins.TryGetValue(coin, out var actualQuantity);

            _grouppedCoins[coin] = actualQuantity - quantity;
        }

        public int GetQuantity(CoinType coin)
        {
            _grouppedCoins.TryGetValue(coin, out var quantity);

            return quantity;
        }
        public ReadOnlyDictionary<CoinType, int> GetAll()
        {
            return new ReadOnlyDictionary<CoinType, int>(
                _grouppedCoins
            );
        }
    }
}