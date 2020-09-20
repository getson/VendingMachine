using MediatR;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Core;

namespace VendingMachine.Queries.Handlers
{
    public class GetProductsHandler :
        RequestHandler<GetProducts, IEnumerable<GetProductItemResult>>
    {
        private readonly IVendingMachineProvider _vendingMachineProvider;

        public GetProductsHandler(IVendingMachineProvider vendingMachineProvider)
        {
            _vendingMachineProvider = vendingMachineProvider;
        }

        protected override IEnumerable<GetProductItemResult> Handle(GetProducts request)
        {
            var machine = _vendingMachineProvider.GetVendingMachine();

            var products = machine.GetProductsWithPrices()
                .Select(kvp => new GetProductItemResult(
                            kvp.Key.ToString(),
                            string.Format("{0:N2} Euro", (decimal)kvp.Value / 100)
                       ));

            var amount = (decimal)machine.GetAmountToBePaid() / 100;

            return products;
        }
    }
}