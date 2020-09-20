using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;

namespace VendingMachine.Core
{
    public class PricesProvider
    {
        private readonly Dictionary<Product, int> _prices;

        public PricesProvider(Dictionary<Product, int> prices)
        {
            _prices = prices;
        }

        public void SetPrice(Product product, int price)
        {
            _prices[product] = price;
        }

        public int GetPrice(Product product)
        {
            if (_prices.TryGetValue(product, out var price))
            {
                return price;
            }
            throw new ArgumentException("Product is invalid", nameof(product));
        }

        public ReadOnlyDictionary<Product, int> GetAll()
        {
            return new ReadOnlyDictionary<Product, int>(_prices);
        }
    }
}