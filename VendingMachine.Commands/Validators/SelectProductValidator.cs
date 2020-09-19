using FluentValidation;
using System;
using System.Linq;
using VendingMachine.Core;

namespace VendingMachine.Commands.Validators
{
    public class SelectProductValidator : AbstractValidator<SelectProduct>
    {
        public SelectProductValidator()
        {
            var productNames = Enum.GetNames(typeof(Product));

            RuleFor(x => x.ProductName).NotEmpty();
            RuleFor(x => x.ProductName)
                .Must(x => productNames.Contains(x))
                .WithMessage("Selected product is not valid!");
        }
    }
}