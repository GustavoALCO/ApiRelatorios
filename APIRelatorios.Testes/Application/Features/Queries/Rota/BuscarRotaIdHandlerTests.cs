using APIRelatorios.Application.Features.Queries.Rota;
using APIRelatorios.Application.Features.Querys.Rota.Handler;
using APIRelatorios.Dommain.Enuns;
using APIRelatorios.Dommain.Interfaces.Rota;
using Moq;
using Xunit;

namespace APIRelatorios.Testes.Application.Features.Queries.Rota;

public class BuscarRotaIdHandlerTests
{
    [Fact]
    public async Task Handle_DeveMapearRotaParaDto()
    {
        var id = Guid.NewGuid();
        var rota = new APIRelatorios.Dommain.Entities.Rota(id, "Rota Norte", Concessionarias.ENEL,
            "AL-1", new DateTime(2026, 1, 15), TipoFiscalizacao.Alimentadores);
        var query = new Mock<IRotaQuery>();
        query.Setup(x => x.BuscarRotaID(id)).ReturnsAsync(rota);
        var handler = new BuscarRotaIdHandler(query.Object);

        var result = await handler.Handle(new BuscarRotaIdQuery { RotaId = id }, default);

        Assert.Equal(id, result.RotaId);
        Assert.Equal("Rota Norte", result.NomeRota);
        Assert.Equal("15/01/2026", result.DataInicio);
    }
}
