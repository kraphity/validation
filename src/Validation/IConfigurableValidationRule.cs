namespace Kraphity.Validation
{
    public interface IConfigurableValidationRule
    {
        IConfigurableValidationRule WithErrorMessage(string error);
    }
}
