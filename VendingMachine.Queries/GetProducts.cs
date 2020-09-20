using MediatR;
using System.Collections.Generic;

namespace VendingMachine.Queries
{
    public class GetProducts : IRequest<IEnumerable<GetProductItemResult>>
    {

    }

    public class GetProductItemResult
    {
        public GetProductItemResult(string productName, string price)
        {
            ProductName = productName;
            Price = price;
        }

        public string ProductName { get; }
        public string Price { get; }
    }

}