using MediatR;
using System;

namespace VendingMachine.Queries
{
    public class GetSelectedProductPrice : IRequest<GetSelectedProductPriceResult>
    {
    }

    public class GetSelectedProductPriceResult
    {
        public GetSelectedProductPriceResult(string amount)
        {
        }

        public string Amount { get; }
    }
}
