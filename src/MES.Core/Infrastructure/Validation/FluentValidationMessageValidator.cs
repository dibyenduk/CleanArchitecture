using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using FluentValidation.Results;

namespace MES.Core.Infrastructure.Validation
{
    public class FluentValidationMessageValidator<T> : IMessageValidator<T>
    {
        private readonly IEnumerable<IValidator<T>> validators;

        public FluentValidationMessageValidator(IEnumerable<IValidator<T>> validators)
        {
            this.validators = validators;

            // ValidatorOptions.ResourceProviderType = typeof(FluentValidationMessageResourceProvider);
        }

        public IList<ValidationFailure> Validate(T message)
        {
            var context = new ValidationContext<T>(message);

            var failures = this.validators
                                .Select(v => v.Validate(context))
                                .SelectMany(result => result.Errors)
                                .Where(f => f != null)
                                .ToList();

            return failures;
        }
    }
}
