using APIRelatorios.Application.Features.Commands.Rota;
using APIRelatorios.Application.Interfaces;
using FluentValidation;

namespace APIRelatorios.Application.Validations.Rota;

public class CreateRelatorioWordValidate : AbstractValidator<CreateRelatorioWordCommand>
{
    public CreateRelatorioWordValidate(IValidateIds validateids)
    {
        RuleFor(x => x.Ids)
        .Cascade(CascadeMode.Stop)
        .NotNull()
        .NotEmpty();
    }
}
