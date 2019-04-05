using System;
using System.Collections.Generic;
using System.Linq;

namespace Kraphity.Validation
{
    [Serializable]
    public class ValidationResult
    {
        internal static readonly ValidationResult Valid = new ValidationResult(null);

        public IEnumerable<ValidationFailure> Errors { get; }

        public bool IsValid { get; }

        private ValidationResult(IEnumerable<ValidationFailure> errors)
        {
            this.Errors = errors ?? new ValidationFailure[0];
            this.IsValid = !this.Errors.Any();
        }

        internal static ValidationResult From(IEnumerable<ValidationFailure> errors)
        {
            return new ValidationResult(errors);
        }

        internal static ValidationResult Error(ValidationFailure error)
        {
            if (error == null)
            {
                throw new ArgumentNullException(nameof(error));
            }

            return new ValidationResult(new[] { error });
        }

        internal static ValidationResult Error(string message)
        {
            return Error(new ValidationFailure(string.Empty, message));
        }

        internal static ValidationResult Error(string memberName, string message)
        {
            return Error(new ValidationFailure(memberName, message));
        }
    }
}
