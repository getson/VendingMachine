using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Core;
using VendingMachine.Events;

namespace VendingMachine.Commands.Handlers
{
    public class ProcessOrderHandler : IRequestHandler<ProcessOrder>
    {
        private readonly IVendingMachineProvider _vendingMachineProvider;
        private readonly IMediator _mediator;

        public ProcessOrderHandler(
            IVendingMachineProvider vendingMachineProvider,
            IMediator mediator
        )
        {
            _vendingMachineProvider = vendingMachineProvider;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(ProcessOrder request, CancellationToken cancellationToken)
        {
            var machine = _vendingMachineProvider.GetVendingMachine();

            var coinsToReturn = machine.ProcessOrder(request.NoChange);

            await _mediator.Publish(
                new OrderProcessed(machine.GetSelectedProduct(),
                coinsToReturn
            ));

            return Unit.Value;
        }
    }
}