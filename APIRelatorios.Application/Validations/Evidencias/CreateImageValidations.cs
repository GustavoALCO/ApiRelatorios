using APIRelatorios.Application.Features.Commands.Images;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Application.Services;
using FluentValidation;

namespace APIRelatorios.Application.Validations.Evidencias;

public class CreateImageValidations : AbstractValidator<CreateEvidenciaCommand>
{
    public CreateImageValidations(IValidateBase64 validateBase64)
    {
            

        RuleFor(x => x.Endereco)
            .NotEmpty().WithMessage("Endereço não pode ser nulo")
            .NotNull().WithMessage("Endereço não pode ser nulo");

        RuleFor(x => x.Base64)
            .NotEmpty().WithMessage("É obrigatorio passar um Base64 como imagem")
            .NotNull().WithMessage("É obrigatorio passar um Base64 como imagem");

        RuleFor(x => x.TemaFiscalizacao)
            .NotNull().WithMessage("Deve Selecionar um assunto para o tema da fiscalização");
    }
}
