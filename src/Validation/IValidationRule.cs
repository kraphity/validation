using System.Threading.Tasks;

namespace Kraphity.Validation
{
    public interface IValidationRule<in T>
    {
        Task<ValidationResult> EvaluateAsync(T value);
    }
}