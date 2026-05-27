using APIRelatorios.Application.Features.Querys.Rota;
using FluentValidation;

namespace APIRelatorios.Application.Validations.Rota;

public class BuscarRotaFiltersValidate : AbstractValidator<BuscarRotaFiltersQuery>
{
    public BuscarRotaFiltersValidate()
    {
        RuleFor(x => x.pagesize)
            .NotEmpty().WithMessage("É Obrigatorio passar um valor para o tamanho da página")
            .NotNull().WithMessage("É Obrigatorio passar um valor para o tamanho da página")
            .GreaterThan(0)
            .WithMessage("Página deve ser maior que 0");;

        RuleFor(x => x.page)
            .NotNull().WithMessage("É Obrigatorio passar um valor para a página")
            .NotEmpty().WithMessage("É Obrigatorio passar um valor para a página")
            .GreaterThan(0)
            .WithMessage("Página deve ser maior que 0");
    }
}
