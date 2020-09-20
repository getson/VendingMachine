using MediatR;
using System.Linq;
using VendingMachine.Core;
using VendingMachine.Core.Domain;

namespace VendingMachine.Commands.Handlers
{
    public class InsertCoinsHandler : RequestHandler<InsertCoins>
    {
        private readonly IVendingMachineProvider _vendingMachineProvider;

        public InsertCoinsHandler(IVendingMachineProvider vendingMachineProvider)
        {
            _vendingMachineProvider = vendingMachineProvider;
        }

        protected override void Handle(InsertCoins request)
        {
            var machine = _vendingMachineProvider.GetVendingMachine();

            var coins = request.Coins.Select(c => (CoinType)c);

            machine.InsertCoins(coins);

            //TODO return message or publish event
        }
    }
}