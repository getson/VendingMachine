using MediatR;

namespace VendingMachine.Commands
{
    public class InsertCoins : IRequest
    {
        public InsertCoins(params int[] coins)
        {
            Coins = coins;
        }

        public int[] Coins { get; }
    }
}