using System.Threading.Tasks;

namespace Kraphity.Validation.Rules
{
    internal class NotEmptyRule<T> : ConfigurableValidationRuleBase<T>
    {
        private readonly IValidationRule<T> notNullRule;

        public NotEmptyRule(string memberName) 
            : base(memberName, "Value cannot be empty.")
        {
            this.notNullRule = new NotNullRule<T>(memberName);
        }

        protected override Task<bool> OnEvaluateAsync(T value)
        {
            if (value is string s)
            {
                return Task.FromResult(!string.IsNullOrEmpty(s));
            }

            return this.notNullRule
                .EvaluateAsync(value)
                .ContinueWith(p => p.Result.IsValid);
        }
    }
}
