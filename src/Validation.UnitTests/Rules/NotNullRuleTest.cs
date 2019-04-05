using Kraphity.Validation.Rules;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kraphity.Validation.UnitTests.Rules
{
    public class NotNullRuleTest
    {
        [Fact]
        public async Task EvaluateAsync_RefValid_NoError()
        {
            var rule = new NotNullRule<string>("p");

            var result = await rule.EvaluateAsync("asdf");

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task EvaluateAsync_RefInvalid_Error()
        {
            var rule = new NotNullRule<string>("p");

            var result = await rule.EvaluateAsync(null);

            Assert.False(result.IsValid);
            Assert.Equal("p", result.Errors.Single().MemberName);
        }
        
        [Fact]
        public async Task EvaluateAsync_ValueTypeNonDefault_NoError()
        {
            var rule = new NotNullRule<int>("p");

            var result = await rule.EvaluateAsync(10);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task EvaluateAsync_ValueTypeDefault_Error()
        {
            var rule = new NotNullRule<int>("p");

            var result = await rule.EvaluateAsync(0);

            Assert.False(result.IsValid);
            Assert.Equal("p", result.Errors.Single().MemberName);
        }

        [Fact]
        public async Task EvaluateAsync_NullableValueTypeNull_Error()
        {
            var rule = new NotNullRule<int?>("p");

            var result = await rule.EvaluateAsync(null);

            Assert.False(result.IsValid);
            Assert.Equal("p", result.Errors.Single().MemberName);
        }

        [Fact]
        public async Task EvaluateAsync_NullableValueTypeValid_NoError()
        {
            var rule = new NotNullRule<int?>("p");

            var result = await rule.EvaluateAsync(10);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task EvaluateAsync_NullableValueTypeDefault_NoError()
        {
            var rule = new NotNullRule<int?>("p");

            var result = await rule.EvaluateAsync(0);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void IsConfigurableRule()
        {
            var rule = new NotNullRule<string>("p");
            Assert.IsAssignableFrom<IConfigurableValidationRule>(rule);
        }
    }
}
