using MediatR;

namespace VendingMachine.Commands
{
    public class ProcessOrder : IRequest
    {
        public ProcessOrder(bool noChange)
        {
            NoChange = noChange;
        }

        public bool NoChange { get; }
    }
}