using System;

namespace VendingMachine.Core.Domain
{
    public class NotSufficientAmountException : Exception
    {
        public NotSufficientAmountException(string message, string remainingAmount)
            : base(message)
        {
            RemainingAmount = remainingAmount;
        }

        public string RemainingAmount { get; }
    }
}