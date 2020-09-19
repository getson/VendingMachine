using MediatR;
using VendingMachine.Core;

namespace VendingMachine.Commands.Handlers
{
    public class ProcessOrderHandler : RequestHandler<ProcessOrder>
    {
        private readonly IVendingMachineProvider _vendingMachineProvider;

        public ProcessOrderHandler(IVendingMachineProvider vendingMachineProvider)
        {
            _vendingMachineProvider = vendingMachineProvider;
        }

        protected override void Handle(ProcessOrder request)
        {
            var machine = _vendingMachineProvider.GetVendingMachine();

            machine.ProcessOrder();
        }
    }
}