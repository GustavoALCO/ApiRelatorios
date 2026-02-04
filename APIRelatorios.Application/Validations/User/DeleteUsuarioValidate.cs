using APIRelatorios.Application.Features.Commands.User;
using APIRelatorios.Application.Interfaces;
using FluentValidation;

namespace APIRelatorios.Application.Validations.User;

public class DeleteUsuarioValidate : AbstractValidator<DeleteUsuarioCommand>
{
    public DeleteUsuarioValidate(IValidateIds validateIds)
    {
        RuleFor(x => x.idUser)
            .NotEmpty().WithMessage("Deve passar o Id")
            .NotNull().WithMessage("Deve passar o Id")
            ;
    }
}
