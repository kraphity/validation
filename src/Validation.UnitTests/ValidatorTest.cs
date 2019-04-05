using Kraphity.Validation.Rules;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kraphity.Validation.UnitTests
{
    public class ValidatorTest
    {
        [Fact]
        public async Task TryValidateAsync_AllRulesSucceed_ReturnsValidResult()
        {
            var foo = new Foo();

            var ruleMocks = new List<Mock<IValidationRule<Foo>>>
            {
                this.SetupRuleMock(foo, ValidationResult.Valid),
                this.SetupRuleMock(foo, ValidationResult.Valid)
            };

            var validatorStub = new FooValidator(ruleMocks.Select(p => p.Object).ToList());

            var result = await validatorStub.TryValidateAsync(foo);

            ruleMocks.ForEach(p => p.Verify(mock => mock.EvaluateAsync(foo), Times.Once()));
            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task TryValidateAsync_OneRuleFails_ReturnsInvalidResult()
        {
            var foo = new Foo();

            var ruleMocks = new List<Mock<IValidationRule<Foo>>>
            {
                this.SetupRuleMock(foo, ValidationResult.Error("rule1"))
            };

            var validatorStub = new FooValidator(ruleMocks.Select(p => p.Object).ToList());

            var result = await validatorStub.TryValidateAsync(foo);

            ruleMocks.ForEach(p => p.Verify(mock => mock.EvaluateAsync(foo), Times.Once()));
            Assert.False(result.IsValid);
            Assert.Equal("rule1", result.Errors.Single().Message);
        }

        [Fact]
        public async Task TryValidateAsync_ManyRulesFail_ReturnsInvalidResult()
        {
            var foo = new Foo();

            var ruleMocks = new List<Mock<IValidationRule<Foo>>>
            {
                this.SetupRuleMock(foo, ValidationResult.Error("rule1")),
                this.SetupRuleMock(foo, ValidationResult.Error("rule2"))
            };

            var validatorStub = new FooValidator(ruleMocks.Select(p => p.Object).ToList());

            var result = await validatorStub.TryValidateAsync(foo);

            ruleMocks.ForEach(p => p.Verify(mock => mock.EvaluateAsync(foo), Times.Once()));
            Assert.False(result.IsValid);
            Assert.Equal(new[] { "rule1", "rule2" }, result.Errors.Select(p => p.Message));
        }

        [Fact]
        public async Task ValidateAsync_OneRuleFails_Throws()
        {
            var foo = new Foo();

            var ruleMocks = new List<Mock<IValidationRule<Foo>>>
            {
                this.SetupRuleMock(foo, ValidationResult.Error("rule1"))
            };

            var validatorStub = new FooValidator(ruleMocks.Select(p => p.Object).ToList());

            var exception = await Assert.ThrowsAsync<ValidationException>(() => validatorStub.ValidateAsync(foo));

            Assert.NotNull(exception.Result);
            Assert.False(exception.Result.IsValid);
            Assert.Equal("rule1", exception.Result.Errors.Single().Message);
        }

        [Fact]
        public async Task ValidateAsync_AllRulesSucceed_DoesNotThrow()
        {
            var foo = new Foo();

            var ruleMocks = new List<Mock<IValidationRule<Foo>>>
            {
                this.SetupRuleMock(foo, ValidationResult.Valid)
            };

            var validatorStub = new FooValidator(ruleMocks.Select(p => p.Object).ToList());

            await validatorStub.ValidateAsync(foo);
        }

        [Fact]
        public void For_WithMemberAndName_ReturnsMemberValidationRule()
        {
            var validatorStub = new FooValidator();

            var rule = validatorStub.For(p => p.Bar, nameof(Foo.Bar));

            Assert.IsType<MemberValidationRule<Foo, string>>(rule);
        }

        [Fact]
        public void For_WithoutValueExpression_Throws()
        {
            var validatorStub = new FooValidator();

            Assert.Throws<ArgumentNullException>(() => validatorStub.For<string>(null, nameof(Foo.Bar)));
        }

        [Fact]
        public void For_WithoutName_Throws()
        {
            var validatorStub = new FooValidator();

            Assert.Throws<ArgumentNullException>(() => validatorStub.For<string>(p => p.Bar, null));
        }

        [Fact]
        public void AddRule_WithoutRule_Throws()
        {
            var validatorStub = new FooValidator();

            Assert.Throws<ArgumentNullException>(() => validatorStub.AddRule(null));
        }

        private Mock<IValidationRule<T>> SetupRuleMock<T>(T instance, ValidationResult result)
        {
            var mock = new Mock<IValidationRule<T>>();
            mock.Setup(p => p.EvaluateAsync(instance)).ReturnsAsync(result);
            return mock;
        }
    }
}
