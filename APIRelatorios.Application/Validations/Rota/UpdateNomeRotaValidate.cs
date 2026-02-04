using APIRelatorios.Application.Features.Commands.Rota;
using APIRelatorios.Application.Interfaces;
using FluentValidation;

namespace APIRelatorios.Application.Validations.Rota;

public class UpdateNomeRotaValidate :AbstractValidator<UpdateNomeRotaCommand>
{
    public UpdateNomeRotaValidate(IValidateIds validateIds)
    {
        RuleFor(x => x.rotaId)
            .NotEmpty().WithMessage("É necessario passar um id")
            .NotNull().WithMessage("É necessario passar um id")
            .WithMessage("Erro ao identificar rota");

        RuleFor(x => x.rotaId)
            .NotEmpty().WithMessage("É necessario passar um nome para atualizar")
            .NotNull().WithMessage("É necessario passar um nome para atualizar");
    }
}
