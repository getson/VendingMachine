using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Core;
using VendingMachine.Events;

namespace VendinMachine.EventHandlers
{
    public class ReturnCoinsWhenOrderIsCancelled : INotificationHandler<OrderCancelled>
    {
        private readonly ITerminal _terminal;

        public ReturnCoinsWhenOrderIsCancelled(ITerminal terminal)
        {
            _terminal = terminal;
        }

        public Task Handle(OrderCancelled notification, CancellationToken cancellationToken)
        {
            _terminal.WriteLine();

            _terminal.WriteLine("Order Cancelled!");
            _terminal.WriteLine($"Please take your coins: Coins: " +
                $"{string.Join(" ", notification.InsertedCoins.Select(c => (int)c))}"
             );
            _terminal.WriteLine();

            return Task.CompletedTask;
        }
    }
}
