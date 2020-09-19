using System;

namespace VendingMachine.Core.Domain
{
    public class NotFullPaidException : Exception
    {
        public NotFullPaidException(string message, int remainingAmount)
            : base(message)
        {
            RemainingAmount = remainingAmount;
        }

        public int RemainingAmount { get; }
    }
}
