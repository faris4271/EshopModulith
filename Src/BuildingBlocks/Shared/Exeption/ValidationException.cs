namespace Shared.Exeption
{
    public class ValidationException : Exception
    {
        private readonly IEnumerable<ValidationError> _validate;


        public ValidationException(IEnumerable<ValidationError> validate)
        {
            _validate = validate;
        }
    }
}
