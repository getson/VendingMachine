using System;

namespace VendingMachine.Core.Domain
{
    public class NotFullyPaidException : Exception
    {
        public NotFullyPaidException(string message, int remainingAmount)
            : base(message)
        {
            RemainingAmount = remainingAmount;
        }

        public int RemainingAmount { get; }
    }
}