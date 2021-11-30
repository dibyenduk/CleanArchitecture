using System;
using FluentValidation;

namespace MES.Core.Features.ViewProcessOrder
{
    public class SearchValidator : AbstractValidator<SearchQuery>
    {     
        public SearchValidator()
        {           
        }
    }
}
