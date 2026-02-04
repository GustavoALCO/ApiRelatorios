using APIRelatorios.Application.Features.Commands.Images;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Application.Services;
using FluentValidation;

namespace APIRelatorios.Application.Validations.Evidencias;

public class DeleteImageValidation : AbstractValidator<DeleteImageCommands>
{
    public DeleteImageValidation(IValidateIds validateIds)
    {
        RuleFor(x => x.idImage)
            .NotEmpty().WithMessage("É obrigatório passar um id")
            .MustAsync(async (id, cancellation) =>
                await validateIds.EvidenciaExisteAsync(id))
            .WithMessage("Id inválido");
    }
}
