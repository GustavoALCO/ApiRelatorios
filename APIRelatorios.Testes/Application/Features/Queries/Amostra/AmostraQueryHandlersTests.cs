using APIRelatorios.Application.Features.Queries.Amostra;
using APIRelatorios.Application.Features.Queries.Amostra.Handler;
using APIRelatorios.Domain.Interfaces.Amostra;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using AmostraEntity = APIRelatorios.Domain.Entities.Amostra;

namespace APIRelatorios.Testes.Application.Features.Queries.Amostra;

public class AmostraQueryHandlersTests
{
    [Fact]
    public async Task BuscarPorId_DeveMapearAmostraParaDto()
    {
        var amostra = CreateAmostra();
        var query = new Mock<IAmostraQuery>();
        query.Setup(x => x.GetAmostraId(1)).ReturnsAsync(amostra);
        var handler = new BuscarAmostraPorIdHandler(query.Object, Mock.Of<ILogger<BuscarAmostraPorIdHandler>>());

        var result = await handler.Handle(new BuscarAmostraPorIdQuery(1), default);

        Assert.Equal("SEQ-1", result.SeqISA);
        Assert.Equal("Endereço", result.Endereco);
    }

    [Fact]
    public async Task BuscarTodas_DeveMapearListaDeAmostras()
    {
        var rotaId = Guid.NewGuid();
        var query = new Mock<IAmostraQuery>();
        query.Setup(x => x.GetAmostraById(rotaId)).ReturnsAsync([CreateAmostra(rotaId)]);
        var handler = new BuscarTodasAmostrasHandler(Mock.Of<ILogger<BuscarTodasAmostrasHandler>>(), query.Object);

        var result = await handler.Handle(new BuscarTodasAmostrasQuery(rotaId), default);

        Assert.Single(result);
        Assert.Equal("SEQ-1", result.Single().SeqISA);
    }

    private static AmostraEntity CreateAmostra(Guid? rotaId = null)
    {
        var amostra = new AmostraEntity();
        amostra.createEquipamentoAmostra(rotaId ?? Guid.NewGuid(), "SEQ-1", "BASE-1", "10",
            "TUC", "Descrição", "ODI", "Instalação", "Endereço", "Cidade", -23, -46,
            "Conforme", "", "", "", "", "", "", "", "");
        return amostra;
    }
}
