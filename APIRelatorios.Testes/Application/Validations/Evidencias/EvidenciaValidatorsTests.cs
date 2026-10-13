using APIRelatorios.Application.Features.Commands.Images;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Application.Services;
using APIRelatorios.Application.Validations.Evidencias;
using APIRelatorios.Domain.Enuns;
using APIRelatorios.Dommain.Enuns;
using Moq;
using Xunit;

namespace APIRelatorios.Testes.Application.Validations.Evidencias;

public class EvidenciaValidatorsTests
{
    private readonly IValidateBase64 _validateBase64 = new ValidateBase64();
    private readonly IValidateIds _validateIds = Mock.Of<IValidateIds>();

    [Fact]
    public void CreateEvidencia_DeveAceitarTemaESubtemaCompativeis()
    {
        var result = new CreateEvidenciaValidations(_validateBase64).Validate(CreateValidCommand());

        Assert.True(result.IsValid, result.ToString());
    }

    [Fact]
    public void CreateEvidencia_DeveRejeitarBase64Vazio()
    {
        var command = CreateValidCommand() with { Base64 = [] };
        var result = new CreateEvidenciaValidations(_validateBase64).Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "Base64");
    }

    [Fact]
    public void CreateEvidencia_DeveRejeitarImagemForaDoFormatoBase64()
    {
        var command = CreateValidCommand() with { Base64 = ["conteudo-invalido"] };
        var result = new CreateEvidenciaValidations(_validateBase64).Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage.Contains("Base64 válido"));
    }

    [Fact]
    public void CreateEvidencia_DeveRejeitarSubtemaDeOutroTema()
    {
        var command = CreateValidCommand() with
        {
            subTemaFiscalizacao = [(int)SubTemaAlimentadores.CaboPartido]
        };
        var result = new CreateEvidenciaValidations(_validateBase64).Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.ErrorMessage.Contains("subtemas permitidos"));
    }

    [Fact]
    public void UpdateDescricao_DeveRejeitarIdDaEvidenciaVazio()
    {
        var command = new UpdateEvidenciasCommands(Guid.Empty, "Descrição", 1, [11],
            null, null, null, NivelRisco.Normal);
        var result = new UpdateDescricaoImageValidation(_validateIds).Validate(command);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "evidenciaId");
    }

    private static CreateEvidenciaCommand CreateValidCommand() => new(
        Guid.NewGuid(), Guid.NewGuid(), 1, (int)TemaCheck.Vegetacao,
        [(int)SubTemaAlimentadores.VegetacaoEmContatoComRedeEletrica],
        "Poste 12", "AL-001", "Vegetação próxima à rede", ["aW1hZ2Vt"],
        "Rua A", "São Paulo", -23.5, -46.6, DateTime.UtcNow, NivelRisco.Normal);
}
