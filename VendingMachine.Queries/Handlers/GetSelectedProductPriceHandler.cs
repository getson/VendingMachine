using MediatR;
using VendingMachine.Core;

namespace VendingMachine.Queries.Handlers
{
    public class GetSelectedProductPriceHandler :
        RequestHandler<GetSelectedProductPrice, GetSelectedProductPriceResult>
    {
        private readonly IVendingMachineProvider _vendingMachineProvider;

        public GetSelectedProductPriceHandler(IVendingMachineProvider vendingMachineProvider)
        {
            _vendingMachineProvider = vendingMachineProvider;
        }

        protected override GetSelectedProductPriceResult Handle(GetSelectedProductPrice request)
        {
            var machine = _vendingMachineProvider.GetVendingMachine();

            var amount = (decimal)machine.GetAmountToBePaid() / 100;

            return new GetSelectedProductPriceResult(
              string.Format("{0:N2} Euro", amount)
                );
        }
    }
}
