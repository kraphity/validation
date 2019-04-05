using System;
using System.Threading.Tasks;

namespace Kraphity.Validation.Rules
{
    internal class ConfigurableValidationRule<T> : ConfigurableValidationRuleBase<T>
    {
        private readonly Func<T, Task<bool>> condition;

        public ConfigurableValidationRule(Func<T, Task<bool>> condition, string memberName, string errorMessage = null)
            : base(memberName, errorMessage)
        {
            this.condition = condition;
        }

        protected override Task<bool> OnEvaluateAsync(T value)
        {
            return this.condition(value);
        }
    }
}
