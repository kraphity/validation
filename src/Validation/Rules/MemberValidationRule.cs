using System;
using System.Threading.Tasks;

namespace Kraphity.Validation.Rules
{
    internal class MemberValidationRule<TInstance, TMember> : IMemberValidationRule<TInstance, TMember>, IValidationRule<TInstance>
    {
        private readonly Func<TInstance, TMember> member;
        private readonly string name;
        private IValidationRule<TMember> rule;

        public MemberValidationRule(Func<TInstance, TMember> member, string name)
        {
            this.member = member;
            this.name = name;
        }

        public Task<ValidationResult> EvaluateAsync(TInstance value)
        {
            if (this.rule == null)
            {
                throw new InvalidOperationException($"Rule has not been built.");
            }

            var memberValue = this.member(value);

            return this.rule.EvaluateAsync(memberValue);
        }

        public IConfigurableValidationRule Match(Func<TMember, bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return this.MatchAsync(value => Task.FromResult(condition(value)));
        }

        public IConfigurableValidationRule MatchAsync(Func<TMember, Task<bool>> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            var rule = new ConfigurableValidationRule<TMember>(condition, this.name);
            return this.SetConfigurableRule(rule);
        }

        public IConfigurableValidationRule NotNull()
        {
            var rule = new NotNullRule<TMember>(this.name);
            return this.SetConfigurableRule(rule);
        }

        public IConfigurableValidationRule NotEmpty()
        {
            var rule = new NotEmptyRule<TMember>(this.name);
            return this.SetConfigurableRule(rule);
        }

        public void UseValidator(IValidator<TMember> validator)
        {
            if (validator == null)
            {
                throw new ArgumentNullException(nameof(validator));
            }

            this.rule = new CustomValidatorRule<TMember>(validator);
        }

        public void MatchRule(IValidationRule<TMember> rule)
        {
            this.rule = rule ?? throw new ArgumentNullException(nameof(rule));
        }

        private IConfigurableValidationRule SetConfigurableRule<T>(T rule) where T : IValidationRule<TMember>, IConfigurableValidationRule
        {
            this.rule = rule;
            return rule;
        }
    }
}
