using APIRelatorios.Application.Features.Commands.Rota;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Application.Services;
using FluentValidation;

namespace APIRelatorios.Application.Validations.Rota;

public class CreateRotaValidate : AbstractValidator<CreateRotaCommand>
{
    public CreateRotaValidate(IValidateIds validateIds)
    {
        RuleFor(x => x.NomeRota)
            .NotEmpty().WithMessage("Não é possivel deixar o campo de nome vazio");

        RuleFor(x => x.Alimentador)
            .NotEmpty().WithMessage("É obrigatorio passar um Alimentador")
            .NotNull().WithMessage("É obrigatorio passar um Alimentador");

        RuleFor(x => x.Fiscais)
            .NotEmpty().WithMessage("É necessario passar pelo menos um id de fiscal")
            .NotNull().WithMessage("É necessario passar pelo menos um id de fiscal");

        RuleFor(x => x.Concessionarias)
            .NotEmpty().WithMessage("É necessario passar uma Concessionaria")
            .NotNull().WithMessage("É necessario passar uma Concessionaria");

    }
}
