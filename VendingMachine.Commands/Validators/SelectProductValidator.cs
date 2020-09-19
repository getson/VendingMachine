using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace VendinMachine.Commands.Validators
{
    public class SelectProductValidator : AbstractValidator<SelectProduct>
    {
        public SelectProductValidator()
        {
            RuleFor(x => x.ProductName).NotEmpty();
        }
    }
}
