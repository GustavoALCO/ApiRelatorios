using APIRelatorios.Application.Features.Commands.Rota;
using APIRelatorios.Application.Interfaces;
using FluentValidation;

namespace APIRelatorios.Application.Validations.Rota;

public class DeleteRotaValidate : AbstractValidator<DeleteRotaCommand>
{

	public DeleteRotaValidate(IValidateIds validateIds)
	{
		RuleFor(x => x.rotaId)
			.NotNull().WithMessage("Não é possivel deixar nulo")
			.NotEmpty().WithMessage("Não é possivel deixar nulo");
	}
}
