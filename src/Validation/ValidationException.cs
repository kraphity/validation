using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Kraphity.Validation
{
    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationResult Result { get; }

        public ValidationException(ValidationResult result)
            : base(BuildMessage(result))
        {
            this.Result = result;
        }

        public ValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.Result = info.GetValue(nameof(this.Result), typeof(ValidationResult)) as ValidationResult;
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this.Result), this.Result);
            base.GetObjectData(info, context);
        }

        private static string BuildMessage(ValidationResult result)
        {
            var errors = string.Join(Environment.NewLine, result.Errors.Select(p => $"- {p.MemberName}: {p.Message}"));
            return $"Validation failed:{Environment.NewLine}{errors}";
        }
    }
}
