using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using VendingMachine.Core.Domain;

namespace VendingMachine.Core
{
    public class Wallet
    {
        private readonly Dictionary<Coin, int> _grouppedCoins;

        public Wallet(Dictionary<Coin, int> coins)
        {
            _grouppedCoins = coins;
        }

        public void Add(Coin coin, int quantity)
        {
            _grouppedCoins.TryGetValue(coin, out var actualQuantity);

            _grouppedCoins[coin] = actualQuantity + quantity;
        }

        public void Deduct(Coin coin, int quantity = 1)
        {
            _grouppedCoins.TryGetValue(coin, out var actualQuantity);

            _grouppedCoins[coin] = actualQuantity - quantity;
        }

        public int GetQuantity(Coin coin)
        {
            _grouppedCoins.TryGetValue(coin, out var quantity);

            return quantity;
        }
        public ReadOnlyDictionary<Coin, int> GetAll()
        {
            return new ReadOnlyDictionary<Coin, int>(
                _grouppedCoins
            );
        }
    }
}