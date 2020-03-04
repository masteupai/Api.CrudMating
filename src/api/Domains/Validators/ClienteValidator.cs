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
    public class ClienteValidator : AbstractValidator<Cliente>
    {
        public ClienteValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithErrorCode(((int)Validation.UserNameNotInformed).ToString())
                .WithMessage("Cliente's name must be informed");

            RuleFor(x => x.Nome)
                .Length(1, 30)
                .WithErrorCode(((int)Validation.UserNameExceedsLimit).ToString())
                .WithMessage("Cliente's name length must be between 1 and 30 characters");
        }
        protected override void EnsureInstanceNotNull(object cliente)
        {
            if (cliente == null)
            {
                var error = new ValidationFailure("Cliente", "Cliente must be informed", null)
                {
                    ErrorCode = ((int)Validation.FuncionarioNotInformed).ToString()
                };

                throw new ValidationException("Something happened when our server was validating your product", new List<ValidationFailure> { error });
            }
        }
    }
}
