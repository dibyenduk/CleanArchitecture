using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MES.Core.Infrastructure.Validation
{
    public interface IMessageValidator<in T>
    {
        IList<ValidationFailure> Validate(T message);
    }
}
