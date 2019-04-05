using Kraphity.Validation.Rules;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kraphity.Validation.UnitTests.Rules
{
    public class MemberValidationRuleTest
    {
        [Fact]
        public async Task EvaluateAsync_WithoutBuiltRule_Throws()
        {
            var rule = new MemberValidationRule<string, string>(p => p, "foo");

            await Assert.ThrowsAsync<InvalidOperationException>(() => rule.EvaluateAsync(string.Empty));
        }

        [Fact]
        public async Task MatchAsync_WithCondition_CreatesRule()
        {
            var memberName = "foo";
            var rule = new MemberValidationRule<Dummy, string>(p => p.Text, memberName);

            var createdRule = rule.MatchAsync(p => Task.FromResult(false));

            var result = await rule.EvaluateAsync(new Dummy());

            Assert.False(result.IsValid);
            Assert.Equal(memberName, result.Errors.Single().MemberName);
            Assert.IsAssignableFrom<IConfigurableValidationRule>(createdRule);
        }

        [Fact]
        public void MatchAsync_WithoutCondition_Throws()
        {
            var rule = new MemberValidationRule<Dummy, string>(p => p.Text, "foo");
            Assert.Throws<ArgumentNullException>(() => rule.MatchAsync(null));
        }

        [Fact]
        public async Task Match_WithCondition_CreatesRule()
        {
            var memberName = "foo";
            var rule = new MemberValidationRule<Dummy, string>(p => p.Text, memberName);

            var createdRule = rule.Match(p => false);

            var result = await rule.EvaluateAsync(new Dummy());

            Assert.False(result.IsValid);
            Assert.Equal(memberName, result.Errors.Single().MemberName);
            Assert.IsAssignableFrom<IConfigurableValidationRule>(createdRule);
        }

        [Fact]
        public void Match_WithoutCondition_Throws()
        {
            var rule = new MemberValidationRule<Dummy, string>(p => p.Text, "foo");
            Assert.Throws<ArgumentNullException>(() => rule.Match(null));
        }

        [Fact]
        public async Task UseValidator_WithValidator_CreatesRule()
        {
            var memberName = nameof(Dummy.Thingy);
            var expectedValidationResult = ValidationResult.Error(memberName, "error");
            var dummy = new Dummy { Text = null, Thingy = new Thing() };

            var thingValidatorMock = new Mock<IValidator<Thing>>();
            thingValidatorMock.Setup(p => p.TryValidateAsync(dummy.Thingy)).ReturnsAsync(expectedValidationResult);

            var rule = new MemberValidationRule<Dummy, Thing>(p => p.Thingy, memberName);
            rule.UseValidator(thingValidatorMock.Object);

            var result = await rule.EvaluateAsync(dummy);

            thingValidatorMock.Verify(p => p.TryValidateAsync(dummy.Thingy), Times.Once());
            Assert.Equal(expectedValidationResult.IsValid, result.IsValid);
            Assert.Equal(expectedValidationResult.Errors, result.Errors);
        }

        [Fact]
        public void UseValidator_WithoutValidator_Throws()
        {
            var rule = new MemberValidationRule<Dummy, string>(p => p.Text, "foo");
            Assert.Throws<ArgumentNullException>(() => rule.UseValidator(null));
        }

        [Fact]
        public async Task NotNull_CreatesRule()
        {
            var memberName = nameof(Dummy.Text);
            var builder = new MemberValidationRule<Dummy, string>(p => p.Text, memberName);

            var rule = builder.NotNull();

            var result = await builder.EvaluateAsync(new Dummy { Text = null });

            Assert.IsType<NotNullRule<string>>(rule);
            Assert.False(result.IsValid);
            Assert.Equal(memberName, result.Errors.Single().MemberName);
        }

        [Fact]
        public async Task NotEmpty_CreatesRule()
        {
            var memberName = nameof(Dummy.Text);
            var builder = new MemberValidationRule<Dummy, string>(p => p.Text, memberName);

            var rule = builder.NotEmpty();

            var result = await builder.EvaluateAsync(new Dummy { Text = string.Empty });

            Assert.IsType<NotEmptyRule<string>>(rule);
            Assert.False(result.IsValid);
            Assert.Equal(memberName, result.Errors.Single().MemberName);
        }

        [Fact]
        public async Task MatchRule_WithRule_SetsRule()
        {
            var memberName = "bar";
            var expectedValidationResult = ValidationResult.Error(memberName, "error");
            var expectedValue = "foo";

            var validationRuleMock = new Mock<IValidationRule<string>>();
            validationRuleMock.Setup(p => p.EvaluateAsync(expectedValue)).ReturnsAsync(expectedValidationResult);

            var rule = new MemberValidationRule<string, string>(p => p, memberName);
            rule.MatchRule(validationRuleMock.Object);

            var result = await rule.EvaluateAsync(expectedValue);

            validationRuleMock.Verify(p => p.EvaluateAsync(expectedValue), Times.Once());
            Assert.Equal(expectedValidationResult.IsValid, result.IsValid);
            Assert.Equal(expectedValidationResult.Errors, result.Errors);
        }

        [Fact]
        public void MatchRule_WithoutRule_Throws()
        {
            var rule = new MemberValidationRule<Dummy, string>(p => p.Text, "foo");
            Assert.Throws<ArgumentNullException>(() => rule.MatchRule(null));
        }

        public class Dummy
        {
            public string Text { get; set; }

            public Thing Thingy { get; set; }
        }

        public class Thing
        {
            public int Id { get; set; }
        }
    }
}
