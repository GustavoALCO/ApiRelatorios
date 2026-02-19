using APIRelatorios.Application.Features.Commands.User;
using FluentValidation;

namespace APIRelatorios.Application.Validations.User;

public class LoginValidate : AbstractValidator<LoginCommandsCommand>
{
    public LoginValidate()
    {
        RuleFor(x => x.Login)
            .NotEmpty().WithMessage("É necessario passar um login")
            .NotNull().WithMessage("É necessario passar um login");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("É necessario passar uma senha")
            .NotNull().WithMessage("É necessario passar uma senha");
    }
}
