using APIRelatorios.Application.Features.Commands.Images;
using APIRelatorios.Application.Interfaces;
using FluentValidation;

namespace APIRelatorios.Application.Validations.Evidencias;

public class UpdateDescricaoImageValidation : AbstractValidator<UpdateDescricaoImageCommands>
{
    public UpdateDescricaoImageValidation(IValidateIds validateIds)
    {
        RuleFor(x => x.evidenciaId)
            .NotEmpty().WithMessage("É obrigatorio passar o Id para modificar a mensagem")
            .NotNull().WithMessage("É obrigatorio passar o Id para modificar a mensagem");
            

        
    }
}
