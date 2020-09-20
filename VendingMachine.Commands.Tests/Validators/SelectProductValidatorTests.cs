using Xunit;
using FluentValidation.TestHelper;
using VendingMachine.Commands.Validators;
using VendingMachine.Core;
using System.Linq;

namespace VendingMachine.Commands.Tests.Validators
{
    public class SelectProductValidatorTests
    {
        private readonly SelectProductValidator _validator;
        public SelectProductValidatorTests()
        {
            _validator = new SelectProductValidator();
        }

        [Fact]
        public void ShouldHaveErrorWhenProductNameIsNull()
        {
            var model = new SelectProduct(null);
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(product => product.ProductName);
        }

        [Fact]
        public void ShouldHaveErrorWhenProductNameIsEmpty()
        {
            var model = new SelectProduct(string.Empty);
            var result = _validator.TestValidate(model);
            Assert.True(result.ShouldHaveValidationErrorFor(product => product.ProductName).Any());
        }

        [Fact]
        public void ShouldHaveErrorWhenProductNameIsNotCorrect()
        {
            var model = new SelectProduct("IncorrectProductName");
            var result = _validator.TestValidate(model);
            Assert.True(result.ShouldHaveValidationErrorFor(product => product.ProductName).Any());
        }

        [Fact]
        public void ShouldNotHaveErrorWhenProductNameIsCorrect()
        {
            var model = new SelectProduct(nameof(Product.ChickenSoup));
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(product => product.ProductName);
            Assert.False(result.Errors.Any());
        }
    }
}
