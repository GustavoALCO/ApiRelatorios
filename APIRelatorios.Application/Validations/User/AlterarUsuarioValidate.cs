using APIRelatorios.Application.Features.Commands.User;
using APIRelatorios.Application.Interfaces;
using FluentValidation;

namespace APIRelatorios.Application.Validations.User;

public class AlterarUsuarioValidate : AbstractValidator<AlterarUsuarioCommand>
{
    public AlterarUsuarioValidate(IValidateIds validateIds)
    {
        RuleFor(x => x.userId)
            .NotEmpty().WithMessage("Deve passar o Id")
            .NotNull().WithMessage("Deve passar o Id")
            ;
    }
}
