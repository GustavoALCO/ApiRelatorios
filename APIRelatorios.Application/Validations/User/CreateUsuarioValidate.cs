using APIRelatorios.Application.Features.Commands.User;
using FluentValidation;

namespace APIRelatorios.Application.Validations.User;

public class CreateUsuarioValidate : AbstractValidator<CreateUsuarioCommand>
{
    public CreateUsuarioValidate()
    {

        RuleFor(x => x.nome)
            .NotNull().WithMessage("Deve passar pelo menos um nome para o login")
            .NotEmpty().WithMessage("Deve passar pelo menos um nome para o login");

        RuleFor(x => x.senha)
            .NotNull().WithMessage("Deve passar pelo menos uma senha para o login")
            .NotEmpty().WithMessage("Deve passar pelo menos uma senha para o login");

        RuleFor(x => x.isAdmin)
            .NotNull().WithMessage("Deve passar se o usuario vai ser admin ou não");
    }
}
