using FluentValidation;
using MediatR.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MES.Core.Infrastructure.Validation
{
    public class ValidationProcessorBehavior<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly IMessageValidator<TRequest> validator;

        public ValidationProcessorBehavior(IMessageValidator<TRequest> validator)
        {
            this.validator = validator;
        }
        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            // throw new NotImplementedException();
            var failures = validator.Validate(request);

            if (failures.Any())
                throw new ValidationException(failures);

            return Task.CompletedTask;
        }
    }
}
