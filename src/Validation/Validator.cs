using Kraphity.Validation.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kraphity.Validation
{
    public abstract class Validator<T> : IValidator<T>
    {
        private readonly IList<IValidationRule<T>> rules;

        protected Validator()
        {
            this.rules = new List<IValidationRule<T>>();
        }

        public async Task<ValidationResult> TryValidateAsync(T value)
        {
            var ruleTasks = this.rules.Select(p => p.EvaluateAsync(value));

            var ruleResults = await Task.WhenAll(ruleTasks)
                .ConfigureAwait(false);

            var errors = ruleResults
                .Where(p => !p.IsValid)
                .SelectMany(p => p.Errors)
                .ToList();

            return ValidationResult.From(errors);
        }

        public async Task ValidateAsync(T value)
        {
            var validationResult = await this.TryValidateAsync(value)
                .ConfigureAwait(false);

            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult);
            }
        }

        protected Validator<T> AddRule(IValidationRule<T> rule)
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            this.rules.Add(rule);
            return this;
        }

        protected IMemberValidationRule<T, TMember> For<TMember>(Func<T, TMember> member, string name)
        {
            if(member == null)
            {
                throw new ArgumentNullException(nameof(member));
            }

            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var rule = new MemberValidationRule<T, TMember>(member, name);
            this.rules.Add(rule);
            return rule;
        }
    }
}