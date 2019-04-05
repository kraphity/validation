using Kraphity.Validation.Rules;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace Kraphity.Validation.UnitTests.Rules
{
    public class CustomValidatorRuleTest
    {
        [Fact]
        public async Task EvaluateAsync_CallsTryValidateOnValidator()
        {
            var expectedResult = ValidationResult.Error("foo");
            var value = "test";
            var validatorMock = new Mock<IValidator<string>>();
            validatorMock.Setup(p => p.TryValidateAsync(value)).ReturnsAsync(expectedResult);

            var rule = new CustomValidatorRule<string>(validatorMock.Object);
            var result = await rule.EvaluateAsync(value);

            validatorMock.Verify(p => p.TryValidateAsync(value), Times.Once());
            Assert.Equal(expectedResult, result);
        }
    }
}
