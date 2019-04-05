using Kraphity.Validation.Rules;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kraphity.Validation.UnitTests.Rules
{
    public class ConfigurableValidationRuleTest
    {
        [Fact]
        public async Task EvaluateAsync_EvaluatesCondition()
        {
            var expectedValue = "bar";
            string actualValue = null;
             
            var validationRule = new ConfigurableValidationRule<string>(value =>
            {
                actualValue = value;
                return Task.FromResult(true);
            }, "foo");

            var result = await validationRule.EvaluateAsync(expectedValue);

            Assert.Equal(expectedValue, actualValue);
        }

        [Fact]
        public async Task EvaluateAsync_MatchingCondition_ReturnsValidResult()
        {
            var rule = new ConfigurableValidationRule<string>(p => Task.FromResult(true), "foo");
            var result = await rule.EvaluateAsync("bar");

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task EvaluateAsync_NotMatchingCondition_ReturnsErrorResult()
        {
            var expectedError = "error";
            var expectedMemberName = "foo";

            var rule = new ConfigurableValidationRule<string>(p => Task.FromResult(false), expectedMemberName, expectedError);

            var result = await rule.EvaluateAsync("bar");

            Assert.False(result.IsValid);
            Assert.Equal(expectedError, result.Errors.Single().Message);
            Assert.Equal(expectedMemberName, result.Errors.Single().MemberName);
        }

        [Fact]
        public async Task EvaluateAsync_WithCustomErrorMessasge_ReturnsErrorMessage()
        {
            var expectedError = "error";
            var expectedMemberName = "foo";

            var rule = new ConfigurableValidationRule<string>(p => Task.FromResult(false), expectedMemberName);
            rule.WithErrorMessage(expectedError);

            var result = await rule.EvaluateAsync("bar");

            Assert.False(result.IsValid);
            Assert.Equal(expectedError, result.Errors.Single().Message);
            Assert.Equal(expectedMemberName, result.Errors.Single().MemberName);
        }

        private class Dummy
        {
            public string Text { get; set; }
        }
    }
}
