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
    public class ContatoClienteValidator : AbstractValidator<ContatoCliente>
    {
        public ContatoClienteValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithErrorCode(((int)Validation.UserNameNotInformed).ToString())
                .WithMessage("Cliente's name must be informed");

            RuleFor(x => x.Email)
                .Length(1, 30)
                .WithErrorCode(((int)Validation.UserNameExceedsLimit).ToString())
                .WithMessage("Cliente's name length must be between 1 and 30 characters");
            RuleFor(x => x.Telefone)
                .NotEmpty()
                .WithErrorCode(((int)Validation.UserNameNotInformed).ToString())
                .WithMessage("Cliente's name must be informed");

            RuleFor(x => x.Telefone)
                .Length(1, 30)
                .WithErrorCode(((int)Validation.UserNameExceedsLimit).ToString())
                .WithMessage("Cliente's name length must be between 1 and 30 characters");
        }
        protected override void EnsureInstanceNotNull(object contatoCliente)
        {
            if (contatoCliente == null)
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
