using MediatR;

namespace VendingMachine.Queries
{
    public class GetSelectedProductPrice : IRequest<GetSelectedProductPriceResult>
    {
    }

    public class GetSelectedProductPriceResult
    {
        public GetSelectedProductPriceResult(string amount)
        {
            Amount = amount;
        }

        public string Amount { get; }
    }
   
}