using System.Threading.Tasks;

namespace Kraphity.Validation
{
    public interface IValidator<in T>
    {
        Task<ValidationResult> TryValidateAsync(T value);

        Task ValidateAsync(T value);
    }
}
