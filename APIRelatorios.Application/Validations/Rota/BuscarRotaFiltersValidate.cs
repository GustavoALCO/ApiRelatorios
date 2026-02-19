using APIRelatorios.Application.Features.Querys.Rota;
using FluentValidation;

namespace APIRelatorios.Application.Validations.Rota;

public class BuscarRotaFiltersValidate : AbstractValidator<BuscarRotaFiltersCommands>
{
    public BuscarRotaFiltersValidate()
    {
        RuleFor(x => x.pagesize)
            .NotEmpty().WithMessage("É Obrigatorio passar um valor para o tamanho da página")
            .NotNull().WithMessage("É Obrigatorio passar um valor para o tamanho da página");

        RuleFor(x => x.page)
            .NotNull().WithMessage("É Obrigatorio passar um valor para a página")
            .NotEmpty().WithMessage("É Obrigatorio passar um valor para a página");
    }
}
