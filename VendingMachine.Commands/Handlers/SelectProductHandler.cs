using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Core;
using VendingMachine.Events;

namespace VendingMachine.Commands.Handlers
{
    public class SelectProductHandler : IRequestHandler<SelectProduct>
    {
        private readonly IVendingMachineProvider _vendingMachineProvider;
        private readonly IMediator _mediator;

        public SelectProductHandler(
            IVendingMachineProvider vendingMachineProvider,
            IMediator mediator
        )
        {
            _vendingMachineProvider = vendingMachineProvider;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SelectProduct request, CancellationToken cancellationToken)
        {
            var machine = _vendingMachineProvider.GetVendingMachine();

            machine.SelectProduct(Enum.Parse<Product>(request.ProductName));

            var priceInEuro = (decimal)machine.GetAmountToBePaid() / 100;

            await _mediator.Publish(
                new ProductSelected(
                    machine.GetSelectedProduct().ToString(),
                    string.Format("{0:N2} Euro", priceInEuro)
                )
            );

            return Unit.Value;
        }
    }
}