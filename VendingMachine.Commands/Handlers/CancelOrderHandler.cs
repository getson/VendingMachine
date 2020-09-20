using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Core;
using VendingMachine.Events;

namespace VendingMachine.Commands.Handlers
{
    public class CancelOrderHandler : IRequestHandler<CancelOrder>
    {
        private readonly IVendingMachineProvider _vendingMachineProvider;
        private readonly IMediator _mediator;

        public CancelOrderHandler(
            IVendingMachineProvider vendingMachineProvider,
            IMediator mediator
         )
        {
            _vendingMachineProvider = vendingMachineProvider;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CancelOrder request, CancellationToken cancellationToken)
        {
            var machine = _vendingMachineProvider.GetVendingMachine();

            var insertedCoins = machine.CancelOrder();

            await _mediator.Publish(new OrderCancelled(insertedCoins));

            return Unit.Value;
        }
    }
}