using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Core;
using VendingMachine.Events;

namespace VendinMachine.EventHandlers
{
    public class ReturnProductAndCoinsWhenOrderIsProcessed :
        INotificationHandler<OrderProcessed>
    {
        private readonly ITerminal _terminal;

        public ReturnProductAndCoinsWhenOrderIsProcessed(ITerminal terminal)
        {
            _terminal = terminal;
        }

        public Task Handle(OrderProcessed notification, CancellationToken cancellationToken)
        {
            _terminal.WriteLine();
            _terminal.WriteLine("Thank you!");
            _terminal.WriteLine();

            _terminal.WriteLine("Please take the product and change!");
            _terminal.WriteLine($"Product: {notification.Product}");
            _terminal.WriteLine($"Coins: " +
                $"{string.Join(" ", notification.CoinsToReturn.Select(c => (int)c))}"
            );

            _terminal.WriteLine();

            return Task.CompletedTask;
        }
    }
}
