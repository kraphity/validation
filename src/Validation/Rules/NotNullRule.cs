using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kraphity.Validation.Rules
{
    internal class NotNullRule<TValue> : ConfigurableValidationRuleBase<TValue>
    {
        public NotNullRule(string memberName)
            : base(memberName, "Value is required.")
        {
        }

        protected override Task<bool> OnEvaluateAsync(TValue value)
        {
            return Task.FromResult(!EqualityComparer<TValue>.Default.Equals(value, default));
        }
    }
}
