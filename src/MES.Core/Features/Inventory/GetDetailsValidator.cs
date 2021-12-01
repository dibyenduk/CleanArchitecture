using System;
using FluentValidation;

namespace MES.Core.Features.Inventory
{
    public class GetDetailsValidator : AbstractValidator<GetDetailsQuery>
    {     
        public GetDetailsValidator()
        {
            RuleFor(t => t.MaterialNbr).NotEmpty();
        }
    }
}
