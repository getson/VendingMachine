using System;
using System.Collections.Generic;
using System.Text;

namespace VendinMachine.Core
{
    public class VendingMachineProvider : IVendingMachineProvider
    {
        private VendingMachine _vendingMachine;
        public VendingMachine GetVendingMachine()
        {

            if (_vendingMachine == null)
            {
                _vendingMachine = new VendingMachine(
                    new Dictionary<Coin, int>
                    {
                        {Coin.TenCent,100 },
                        {Coin.TwentyCent,100 },
                        {Coin.HalfEuro,100 },
                        {Coin.OneEuro,100 }
                    },
                    new Dictionary<Product, int>
                    {
                        {Product.Tea,10 },
                        {Product.Esspresso,20 },
                        {Product.Juice,20 },
                        {Product.ChickenSoup,15 },
                    }
                );
            }

            return _vendingMachine;
        }
    }
}
