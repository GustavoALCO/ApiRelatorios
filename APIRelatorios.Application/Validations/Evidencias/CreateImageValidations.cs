using APIRelatorios.Application.Features.Commands.Images;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Application.Services;
using FluentValidation;

namespace APIRelatorios.Application.Validations.Evidencias;

public class CreateImageValidations : AbstractValidator<CreateImageCommand>
{
    public CreateImageValidations(IValidateBase64 validateBase64)
    {
        RuleFor(x => x.Cep)
            .NotEmpty().WithMessage("Cep não pode ser nulo")
            .NotNull().WithMessage("Cep não pode ser nulo")
            .Matches(@"^\d{5}-\d{3}$").WithMessage("O CEP deve estar no formato 00000-000");          

        RuleFor(x => x.Endereco)
            .NotEmpty().WithMessage("Endereço não pode ser nulo")
            .NotNull().WithMessage("Endereço não pode ser nulo");

        RuleFor(x => x.Base64)
            .NotEmpty().WithMessage("É obrigatorio passar um Base64 como imagem")
            .NotNull().WithMessage("É obrigatorio passar um Base64 como imagem");

        RuleFor(x => x.Endereco)
            .NotEmpty().WithMessage("Endereço não pode ser nulo")
            .NotNull().WithMessage("Endereço não pode ser nulo");

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("Deve conter uma mensagem do assunto")
            .NotNull().WithMessage("Deve conter uma mensagem do assunto");

        RuleFor(x => x.TemaFiscalizacao)
            .NotEmpty().WithMessage("Deve Selecionar um assunto para o tema da fiscalização")
            .NotNull().WithMessage("Deve Selecionar um assunto para o tema da fiscalização");
    }
}
