using MediatR;
using VendingMachine.Core;

namespace VendingMachine.Commands.Handlers
{
    public class CancelOrderHandler : RequestHandler<CancelOrder>
    {
        private readonly IVendingMachineProvider _vendingMachineProvider;

        public CancelOrderHandler(IVendingMachineProvider vendingMachineProvider)
        {
            _vendingMachineProvider = vendingMachineProvider;
        }

        protected override void Handle(CancelOrder request)
        {
            var machine = _vendingMachineProvider.GetVendingMachine();

            machine.ProcessOrder();
        }
    }
}