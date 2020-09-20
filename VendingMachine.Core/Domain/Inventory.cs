using System.Collections.Generic;

namespace VendingMachine.Core
{
    public class Inventory
    {
        private readonly Dictionary<Product, int> _stock;

        public Inventory(Dictionary<Product, int> stock)
        {
            _stock = stock;
        }

        public void Add(Product product, int portions)
        {
            _stock.TryGetValue(product, out var actualPortions);

            _stock[product] = actualPortions + portions;
        }

        public int GetStock(Product product)
        {
            _stock.TryGetValue(product, out var stock);

            return stock;
        }

        public void Deduct(Product product)
        {
            _stock.TryGetValue(product, out var actualPortions);

            _stock[product] = actualPortions - 1;
        }
    }
}