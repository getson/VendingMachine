using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VendingMachine.Core;
using VendingMachine.Core.Domain;

namespace VendingMachine.Commands.Handlers
{
    public class InsertCoinsHandler : IRequestHandler<InsertCoins>
    {
        private readonly IVendingMachineProvider _vendingMachineProvider;

        public InsertCoinsHandler(IVendingMachineProvider vendingMachineProvider)
        {
            _vendingMachineProvider = vendingMachineProvider;
        }

        public Task<Unit> Handle(InsertCoins request, CancellationToken cancellationToken)
        {
            var machine = _vendingMachineProvider.GetVendingMachine();

            var coins = request.Coins.Select(c => (Coin)c);

            machine.InsertCoins(coins);

            return Task.FromResult(Unit.Value);
        }
    }
}