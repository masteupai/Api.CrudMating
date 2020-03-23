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
    public class ServicoProdutoValidator : AbstractValidator<ServicoProduto>
    {
        public ServicoProdutoValidator()
        {

            RuleFor(x => x.ServicoId)
                .NotEmpty()
                .WithErrorCode(((int)Validation.UserNameNotInformed).ToString())
                .WithMessage("Servico's Data must be informed");

            RuleFor(x => x.ProdutoId)
                 .NotEmpty()
                .WithErrorCode(((int)Validation.UserNameNotInformed).ToString())
                .WithMessage("Servico's Quilometragem must be informed");
        }


        protected override void EnsureInstanceNotNull(object servico)
        {
            if (servico == null)
            {
                var error = new ValidationFailure("Servico", "Servico must be informed", null)
                {
                    ErrorCode = ((int)Validation.UserNotInformed).ToString()
                };

                throw new ValidationException("Something happened when our server was validating your servico", new List<ValidationFailure> { error });
            }
        }
    }
}
