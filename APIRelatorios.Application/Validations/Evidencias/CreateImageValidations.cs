using APIRelatorios.Application.Features.Commands.Images;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Enuns;
using APIRelatorios.Dommain.Helpers;
using FluentValidation;

namespace APIRelatorios.Application.Validations.Evidencias;

public class CreateEvidenciaValidations
    : AbstractValidator<CreateEvidenciaCommand>
{
    public CreateEvidenciaValidations(
        IValidateBase64 validateBase64)
    {
        RuleFor(x => x.Base64)
            .NotEmpty()
            .WithMessage(
                "É obrigatorio passar um Base64 como imagem");

        RuleFor(x => x.Latitude)
            .NotNull()
            .WithMessage(
                "É obrigatorio passar a latitude");

        RuleFor(x => x.Longitude)
            .NotNull()
            .WithMessage(
                "É obrigatorio passar a Longitude");

        RuleFor(x => x.temaFiscalizacao)
            .Must(x =>
                Enum.IsDefined(typeof(TemaCheck), x))
            .WithMessage(
                "Tema da fiscalização inválido");

        RuleFor(x => x.subTemaFiscalizacao)
            .NotNull()
            .NotEmpty()
            .WithMessage(
                "É obrigatorio informar ao menos um subtema");

        RuleForEach(x => x.subTemaFiscalizacao)
            .Must(x =>
                Enum.IsDefined(
                    typeof(SubTemaAlimentadores),
                    x))
            .WithMessage(
                "Subtema da fiscalização inválido");

        RuleFor(x => x)
            .Must(x =>
            {
                var subTemas =
                    x.subTemaFiscalizacao
                        .Select(subTema =>
                            (SubTemaAlimentadores)subTema)
                        .ToList();

                return TemaFiscalizacaoMapper
                    .ValidarSubTemas(
                        (TemaCheck)x.temaFiscalizacao,
                        subTemas);
            })
            .WithMessage(x =>
                TemaFiscalizacaoMapper
                    .ObterMensagem(
                        (TemaCheck)x.temaFiscalizacao));
    }
}