using System;

namespace VendingMachine.Core.Domain
{
    public class NotSufficentAmountException : Exception
    {
        public NotSufficentAmountException(string message, int remainingAmount)
            : base(message)
        {
            RemainingAmount = remainingAmount;
        }

        public int RemainingAmount { get; }
    }
}