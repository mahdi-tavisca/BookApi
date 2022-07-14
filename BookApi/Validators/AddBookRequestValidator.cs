using BookApi.Contracts;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookApi.Validators
{
    public class AddBookRequestValidator : AbstractValidator<Book>
    {
        public AddBookRequestValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(request => request.Name)
                .NotNull().WithMessage("Name is required")
                .NotEmpty().WithMessage("Name should not be empty");

            RuleFor(request => request.Author)
               .NotNull().WithMessage("Author is required")
               .NotEmpty().WithMessage("Author should not be empty");

            RuleFor(request => request.BookId)
               .NotNull().WithMessage("BookId is required")
               .NotEmpty().WithMessage("BookId should not be empty");

            RuleFor(request => request.Type.ToLower())
               .NotNull().WithMessage("Author is required")
               .NotEmpty().WithMessage("Author should not be empty")
               .NotEqual("horror").WithMessage("Type should not be Horror").WithErrorCode("123")
               .NotEqual("adventure").WithMessage("Type should not be Adventure").WithErrorCode("123")
               .NotEqual("romance").WithMessage("Type should not be Romance").WithErrorCode("123");
        }
    }
}
