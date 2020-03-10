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
    public class EnderecoFuncionarioValidator : AbstractValidator<EnderecoFuncionario>
    {
        public EnderecoFuncionarioValidator()
        {
            RuleFor(x => x.CEP)
                .NotEmpty()
                .WithErrorCode(((int)Validation.EnderecoNotInformed).ToString())
                .WithMessage("CEP's must be informed");

        }
        protected override void EnsureInstanceNotNull(object contatoFuncionario)
        {
            if (contatoFuncionario == null)
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
