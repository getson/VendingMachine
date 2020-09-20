using MediatR;

namespace VendingMachine.Events
{
    public class ProductSelected : INotification
    {
        public ProductSelected(string product, string price)
        {
            Product = product;
            Price = price;
        }

        public string Product { get; }
        public string Price { get; }
    }
}