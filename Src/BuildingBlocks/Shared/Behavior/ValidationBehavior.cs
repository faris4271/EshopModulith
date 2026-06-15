
using FluentValidation;
using MediatR;
using Shared.Contract.Exeption;
using ValidationException = Shared.Contract.Exeption.ValidationException;





namespace Shared.Behavior
{
    public sealed class ValidationBehavior<TRequest, TRespons> : IPipelineBehavior<TRequest, TRespons>
    {
        private readonly IEnumerable<IValidator> _validators;
        public ValidationBehavior(IEnumerable<IValidator> validators)
        {
            _validators = validators;
        }
        public Task<TRespons> Handle(TRequest request, RequestHandlerDelegate<TRespons> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any()) return next();

            var context = new ValidationContext<TRequest>(request);

            var validationErrors = _validators
          .Select(validator => validator.Validate(context))
          .Where(validationResult => validationResult.Errors.Any())
          .SelectMany(validationResult => validationResult.Errors)
          .Select(validationFailure => new ValidationError(
              validationFailure.PropertyName,
              validationFailure.ErrorMessage))
          .ToList();
            if (validationErrors.Any()) throw new ValidationException(validationErrors);

            return next();



        }
    }
}
