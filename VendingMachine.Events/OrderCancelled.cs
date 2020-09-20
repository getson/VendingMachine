using MediatR;
using System.Collections.Generic;
using VendingMachine.Core.Domain;

namespace VendingMachine.Events
{
    public class OrderCancelled : INotification
    {
        public OrderCancelled(IReadOnlyCollection<Coin> insertedCoins)
        {
            InsertedCoins = insertedCoins;
        }

        public IReadOnlyCollection<Coin> InsertedCoins { get; }
    }
}