using System;

namespace Kraphity.Validation
{
    [Serializable]
    public class ValidationFailure
    {
        public string MemberName { get; }
        public string Message { get; }

        public ValidationFailure(string memberName, string message)
        {
            this.MemberName = memberName ?? throw new ArgumentNullException(memberName);
            this.Message = message ?? throw new ArgumentNullException(message);
        }
    }
}
