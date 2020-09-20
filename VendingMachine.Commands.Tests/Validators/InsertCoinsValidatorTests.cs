using Xunit;
using FluentValidation.TestHelper;
using VendingMachine.Commands.Validators;
using System.Linq;

namespace VendingMachine.Commands.Tests
{
    public class InsertCoinsValidatorTests
    {
        private readonly InsertCoinsValidator _validator;
        public InsertCoinsValidatorTests()
        {
            _validator = new InsertCoinsValidator();
        }

        [Fact]
        public void ShouldNotHaveErrorWhenCoinsAreInValid()
        {
            var model = new InsertCoins(10, 20, 50, 100, 200);
            var result = _validator.TestValidate(model);
            Assert.True(result.ShouldHaveValidationErrorFor(coins => coins.Coins).Any());
        }

        [Fact]
        public void ShouldNotHaveErrorWhenCoinsAreValid()
        {
            var model = new InsertCoins(10, 20, 50, 100);
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveValidationErrorFor(coins => coins.Coins);
            Assert.False(result.Errors.Any());
        }
    }
}
