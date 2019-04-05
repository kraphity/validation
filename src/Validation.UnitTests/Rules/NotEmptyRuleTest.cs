using Kraphity.Validation.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kraphity.Validation.UnitTests.Rules
{
    public class NotEmptyRuleTest
    {
        [Fact]
        public async Task EvaluateAsync_EmptyString_ReturnsError()
        {
            var rule = new NotEmptyRule<string>("foo");

            var result = await rule.EvaluateAsync(string.Empty);

            Assert.False(result.IsValid);
            Assert.Equal("foo", result.Errors.Single().MemberName);
        }

        [Fact]
        public async Task EvaluateAsync_NonEmptyString_ReturnsValid()
        {
            var rule = new NotEmptyRule<string>("foo");

            var result = await rule.EvaluateAsync("bar");

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task EvaluateAsync_NullString_ReturnsError()
        {
            var rule = new NotEmptyRule<string>("foo");

            var result = await rule.EvaluateAsync(null);

            Assert.False(result.IsValid);
            Assert.Equal("foo", result.Errors.Single().MemberName);
        }

        [Fact]
        public async Task EvaluateAsync_EmptyGuid_ReturnsError()
        {
            var rule = new NotEmptyRule<Guid>("foo");

            var result = await rule.EvaluateAsync(Guid.Empty);

            Assert.False(result.IsValid);
            Assert.Equal("foo", result.Errors.Single().MemberName);
        }

        [Fact]
        public async Task EvaluateAsync_NonEmptyGuid_ReturnsValid()
        {
            var rule = new NotEmptyRule<Guid>("foo");

            var result = await rule.EvaluateAsync(Guid.NewGuid());

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task EvaluateAsync_EmptyEnumerable_ReturnsError()
        {
            var rule = new NotEmptyRule<IEnumerable<string>>("foo");

            var result = await rule.EvaluateAsync(new string[0]);

            Assert.False(result.IsValid);
            Assert.Equal("foo", result.Errors.Single().MemberName);
        }

        [Fact]
        public async Task EvaluateAsync_NonEmptyEnumerable_ReturnsValid()
        {
            var rule = new NotEmptyRule<IEnumerable<string>>("foo");

            var result = await rule.EvaluateAsync(new[] { "bar" });

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task EvaluateAsync_NullEnumerable_ReturnsError()
        {
            var rule = new NotEmptyRule<IEnumerable<string>>("foo");

            var result = await rule.EvaluateAsync(null);

            Assert.False(result.IsValid);
            Assert.Equal("foo", result.Errors.Single().MemberName);
        }

        [Fact]
        public async Task EvaluateAsync_DefaultValueType_ReturnsError()
        {
            var rule = new NotEmptyRule<int>("foo");

            var result = await rule.EvaluateAsync(default);

            Assert.False(result.IsValid);
            Assert.Equal("foo", result.Errors.Single().MemberName);
        }

        [Fact]
        public async Task EvaluateAsync_NonDefaultValueType_ReturnsValid()
        {
            var rule = new NotEmptyRule<int>("foo");

            var result = await rule.EvaluateAsync(3);

            Assert.True(result.IsValid);
        }
    }
}
