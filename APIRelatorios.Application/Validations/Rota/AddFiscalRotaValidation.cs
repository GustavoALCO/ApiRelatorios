using APIRelatorios.Application.Features.Commands.Rota;
using APIRelatorios.Application.Interfaces;
using FluentValidation;

namespace APIRelatorios.Application.Validations.Rota;

public class AddFiscalRotaValidation : AbstractValidator<AddFiscalRotaCommand>
{
    public AddFiscalRotaValidation(IValidateIds validateIds)
    {
        RuleFor(x => x.rotaId)
            .NotNull().WithMessage("É necessario passar um id de rota")
            .NotEmpty().WithMessage("É necessario passar um id de rota");

        RuleFor(x => x.FiscaisId)
            .NotEmpty().WithMessage("É necessario passar um id de fiscal")
            .NotNull().WithMessage("É necessario passar um id de fiscal");
    }
}
