using API.Domains.Models;
using API.Domains.Models.Faults;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;

namespace API.Domains.Validators
{
    public class VeiculoValidator : AbstractValidator<Veiculo>
    {

        public VeiculoValidator()
        {
            RuleFor(x => x.Marca)
                .NotEmpty()
                .WithErrorCode(((int)Validation.UserNameNotInformed).ToString())
                .WithMessage("Veiculo's marca must be informed");

            RuleFor(x => x.Marca)
                .Length(1, 80)
                .WithErrorCode(((int)Validation.UserNameExceedsLimit).ToString())
                .WithMessage("Veiculo's marca length must be between 1 and 25 characters");
        }

        protected override void EnsureInstanceNotNull(object veiculo)
        {
            if (veiculo == null)
            {
                var error = new ValidationFailure("Veiculo", "Veiculo must be informed", null)
                {
                    ErrorCode = ((int)Validation.UserNotInformed).ToString()
                };

                throw new ValidationException("Something happened when our server was validating your veiculo", new List<ValidationFailure> { error });
            }
        }
    }
}
