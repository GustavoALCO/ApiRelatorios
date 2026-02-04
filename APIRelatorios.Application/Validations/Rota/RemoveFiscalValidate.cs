using APIRelatorios.Application.Features.Commands.Rota;
using APIRelatorios.Application.Interfaces;
using FluentValidation;

namespace APIRelatorios.Application.Validations.Rota;

public class RemoveFiscalValidate : AbstractValidator<RemoveFiscalRotaCommand>
{
    public RemoveFiscalValidate(IValidateIds validateIds)
    {
        RuleFor(x => x.rotaId)
            .NotNull().WithMessage("É necessario passar um id de rota")
            .NotEmpty().WithMessage("É necessario passar um id de rota");

        RuleFor(x => x.fiscaisId)
            .NotEmpty().WithMessage("É necessario passar um id de fiscal")
            .NotNull().WithMessage("É necessario passar um id de fiscal");
    }
}