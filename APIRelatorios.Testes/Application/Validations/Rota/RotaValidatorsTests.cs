using APIRelatorios.Application.Features.Commands.Rota;
using APIRelatorios.Application.Features.Querys.Rota;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Application.Validations.Rota;
using APIRelatorios.Dommain.Enuns;
using Moq;
using Xunit;

namespace APIRelatorios.Testes.Application.Validations.Rota;

public class RotaValidatorsTests
{
    private readonly IValidateIds _validateIds = Mock.Of<IValidateIds>();

    [Fact]
    public void CreateRota_DeveAceitarComandoValido()
    {
        var result = new CreateRotaValidate(_validateIds).Validate(
            new CreateRotaCommand(Guid.NewGuid(), "Rota Norte", Concessionarias.ENEL,
                "AL-001", [10], TipoFiscalizacao.Alimentadores));

        Assert.True(result.IsValid, result.ToString());
    }

    [Fact]
    public void CreateRota_DeveRejeitarCamposObrigatoriosVazios()
    {
        var result = new CreateRotaValidate(_validateIds).Validate(
            new CreateRotaCommand(Guid.NewGuid(), "", Concessionarias.ENEL,
                "", [], TipoFiscalizacao.Alimentadores));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "NomeRota");
        Assert.Contains(result.Errors, error => error.PropertyName == "Alimentador");
        Assert.Contains(result.Errors, error => error.PropertyName == "Fiscais");
    }

    [Fact]
    public void AddFiscal_DeveRejeitarRotaOuFiscaisAusentes()
    {
        var result = new AddFiscalRotaValidation(_validateIds).Validate(
            new AddFiscalRotaCommand(Guid.Empty, []));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "rotaId");
        Assert.Contains(result.Errors, error => error.PropertyName == "FiscaisId");
    }

    [Fact]
    public void RemoveFiscal_DeveAceitarIdsInformados()
    {
        var result = new RemoveFiscalValidate(_validateIds).Validate(
            new RemoveFiscalRotaCommand(Guid.NewGuid(), [1]));

        Assert.True(result.IsValid, result.ToString());
    }

    [Fact]
    public void UpdateNomeRota_DeveRejeitarNomeVazio()
    {
        var result = new UpdateNomeRotaValidate(_validateIds).Validate(
            new UpdateNomeRotaCommand(Guid.NewGuid(), "", "AL-001"));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "nomeRota");
    }

    [Fact]
    public void CreateRelatorioWord_DeveRejeitarListaDeIdsVazia()
    {
        var result = new CreateRelatorioWordValidate(_validateIds).Validate(
            new CreateRelatorioWordCommand([]));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "Ids");
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(1, 0)]
    public void BuscarRotaFilters_DeveRejeitarPaginacaoInvalida(int page, int pageSize)
    {
        var result = new BuscarRotaFiltersValidate().Validate(new BuscarRotaFiltersQuery
        {
            FiscalId = 1,
            page = page,
            pagesize = pageSize
        });

        Assert.False(result.IsValid);
    }

    [Fact]
    public void BuscarRotaFilters_DeveAceitarPaginacaoPositiva()
    {
        var result = new BuscarRotaFiltersValidate().Validate(new BuscarRotaFiltersQuery
        {
            FiscalId = 1,
            page = 1,
            pagesize = 20
        });

        Assert.True(result.IsValid, result.ToString());
    }
}
