using MediatR;

namespace VendingMachine.Commands
{
    public class SelectProduct : IRequest
    {
        public SelectProduct(string productName)
        {
            ProductName = productName;
        }

        public string ProductName { get; }
    }
}