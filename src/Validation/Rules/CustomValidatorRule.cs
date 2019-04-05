using System.Threading.Tasks;

namespace Kraphity.Validation.Rules
{
    internal class CustomValidatorRule<T> : IValidationRule<T>
    {
        private readonly IValidator<T> validator;

        public CustomValidatorRule(IValidator<T> validator)
        {
            this.validator = validator;
        }

        public Task<ValidationResult> EvaluateAsync(T value)
        {
            return this.validator
                .TryValidateAsync(value)
                .ContinueWith(t => t.Result);
        }
    }
}
