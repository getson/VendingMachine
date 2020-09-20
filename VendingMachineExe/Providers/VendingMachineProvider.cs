using System.Collections.Generic;
using VendingMachine.Core;
using VendingMachine.Core.Domain;
using VendingMachine.Core.Domain.Services;

namespace VendingMachine.CLI.Providers
{
    public class VendingMachineProvider : IVendingMachineProvider
    {
        private readonly IChangeCalculator _changeCalculator;
        private VMachine _vendingMachine;

        public VendingMachineProvider(IChangeCalculator changeCalculator)
        {
            _changeCalculator = changeCalculator;
        }

        public VMachine GetVendingMachine()
        {
            if (_vendingMachine == null)
            {
                var inventory = new Inventory(
                    new Dictionary<Product, int>
                    {
                        {Product.Tea,10 },
                        {Product.Espresso,20 },
                        {Product.Juice,20 },
                        {Product.ChickenSoup,15 },
                    });

                var wallet = new Wallet(
                    new Dictionary<CoinType, int>
                    {
                        {CoinType.TenCent,100 },
                        {CoinType.TwentyCent,100 },
                        {CoinType.HalfEuro,100 },
                        {CoinType.OneEuro,100 }
                    });
                var pricesProvider = new PricesProvider(
                    new Dictionary<Product, int>()
                    {
                        {Product.Tea, 130},
                        {Product.Espresso, 180 },
                        {Product.Juice, 180 },
                        {Product.ChickenSoup, 180 }
                    });

                _vendingMachine = new VMachine(
                    wallet, inventory, pricesProvider,
                    _changeCalculator
                );
            }

            return _vendingMachine;
        }
    }
}