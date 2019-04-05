using System;
using Xunit;

namespace Kraphity.Validation.UnitTests
{
    public class ValidationResultTest
    {
        [Fact]
        public void Valid_IsValidWithoutErrors()
        {
            var result = ValidationResult.Valid;

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void From_WithErrors_NotValidWithErrors()
        {
            var expectedErrors = new[] { new ValidationFailure("a", "foo"), new ValidationFailure("b", "bar") };

            var result = ValidationResult.From(expectedErrors);

            Assert.False(result.IsValid);
            Assert.Equal(expectedErrors, result.Errors);
        }

        [Fact]
        public void From_WithoutErrors_IsValidWithoutErrors()
        {
            var result = ValidationResult.From(new ValidationFailure[0]);

            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public void Error_WithError_NotValidWithErrors()
        {
            var expectedErrors = new[] { new ValidationFailure("a", "foo") };

            var result = ValidationResult.Error(expectedErrors[0]);

            Assert.False(result.IsValid);
            Assert.Equal(expectedErrors, result.Errors);
        }

        [Fact]
        public void Error_WithMessage_NotValidWithErrors()
        {
            var result = ValidationResult.Error("foo");

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, p => p.MemberName == string.Empty && p.Message.Equals("foo", StringComparison.Ordinal));
        }

        [Fact]
        public void Error_WithNameAndMessage_NotValidWithErrors()
        {
            var result = ValidationResult.Error("a", "foo");

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, p => p.MemberName.Equals("a", StringComparison.Ordinal) && p.Message.Equals("foo", StringComparison.Ordinal));
        }
    }
}
