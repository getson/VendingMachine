using MediatR;
using System.Collections.Generic;
using VendingMachine.Core;
using VendingMachine.Core.Domain;

namespace VendingMachine.Events
{
    public class OrderProcessed : INotification
    {
        public OrderProcessed(Product product, IReadOnlyCollection<Coin> coinsToReturn)
        {
            Product = product;
            CoinsToReturn = coinsToReturn;
        }

        public Product Product { get; }
        public IReadOnlyCollection<Coin> CoinsToReturn { get; }
    }
}