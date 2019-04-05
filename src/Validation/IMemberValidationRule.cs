using System;
using System.Threading.Tasks;

namespace Kraphity.Validation
{
    public interface IMemberValidationRule<in T, out TMember>
    {
        IConfigurableValidationRule Match(Func<TMember, bool> condition);
        IConfigurableValidationRule MatchAsync(Func<TMember, Task<bool>> condition);
        void MatchRule(IValidationRule<TMember> rule);
        IConfigurableValidationRule NotNull();
        IConfigurableValidationRule NotEmpty();
        void UseValidator(IValidator<TMember> validator);
    }
}
