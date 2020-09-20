using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Core;
using VendingMachine.Events;

namespace VendingMachine.EventHandlers
{
    public class ShowProductPriceWhenProductIsSelectedEventHandler : INotificationHandler<ProductSelected>
    {
        private readonly ITerminal _terminal;

        public ShowProductPriceWhenProductIsSelectedEventHandler(ITerminal terminal)
        {
            _terminal = terminal;
        }

        public Task Handle(ProductSelected notification, CancellationToken cancellationToken)
        {
            _terminal.WriteLine();
            _terminal.WriteLine($"{notification.Product} - {notification.Price}");
            _terminal.WriteLine();

            return Task.CompletedTask;
        }
    }
}
