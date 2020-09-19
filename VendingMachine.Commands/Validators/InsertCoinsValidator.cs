using FluentValidation;
using System;
using System.Linq;
using VendingMachine.Core.Domain;

namespace VendingMachine.Commands.Validators
{
    public class InsertCoinsValidator : AbstractValidator<InsertCoins>
    {
        public InsertCoinsValidator()
        {
            var coinValues = Enum.GetValues(typeof(Coin))
                            .Cast<int>()
                            .ToArray();

            RuleForEach(x => x.Coins)
                .Must(c => coinValues.Contains(c));
        }
    }
}