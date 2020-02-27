using API.Domains.Models;
using API.Domains.Models.Faults;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Domains.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithErrorCode(((int)Validation.UserNameNotInformed).ToString())
                .WithMessage("Product's name must be informed");

            RuleFor(x => x.Nome)
                .Length(1, 80)
                .WithErrorCode(((int)Validation.UserNameExceedsLimit).ToString())
                .WithMessage("Product's name length must be between 1 and 25 characters");
        }

        protected override void EnsureInstanceNotNull(object product)
        {
            if (product == null)
            {
                var error = new ValidationFailure("User", "User must be informed", null)
                {
                    ErrorCode = ((int)Validation.UserNotInformed).ToString()
                };

                throw new ValidationException("Something happened when our server was validating your product", new List<ValidationFailure> { error });
            }
        }
    }
}
