using System.Threading.Tasks;

namespace Kraphity.Validation.Rules
{
    internal abstract class ConfigurableValidationRuleBase<T> : IValidationRule<T>, IConfigurableValidationRule
    {
        private const string DefaultErrorMessage = "Rule evalulation failed.";

        private readonly string memberName;
        private string errorMessage;

        public ConfigurableValidationRuleBase(string memberName, string errorMessage = null)
        {
            this.memberName = memberName;
            this.errorMessage = errorMessage;
        }

        public Task<ValidationResult> EvaluateAsync(T value)
        {
            return this.OnEvaluateAsync(value)
                .ContinueWith(p => p.Result
                    ? ValidationResult.Valid
                    : ValidationResult.Error(this.BuildValidationFailure()));
        }

        public IConfigurableValidationRule WithErrorMessage(string message)
        {
            this.errorMessage = message;
            return this;
        }

        protected abstract Task<bool> OnEvaluateAsync(T value);

        private ValidationFailure BuildValidationFailure()
        {
            return new ValidationFailure(this.memberName, this.errorMessage ?? DefaultErrorMessage);
        }
    }
}
