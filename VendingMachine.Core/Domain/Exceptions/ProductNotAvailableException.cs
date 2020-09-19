using System;

namespace VendingMachine.Core.Domain
{
    public class ProductNotAvailableException : Exception
    {
        public ProductNotAvailableException(string message)
            : base(message)
        {
        }
    }
}