using APIRelatorios.Application.Features.Commands.Rota;
using APIRelatorios.Application.Features.Commands.Rota.Handler;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Enuns;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.User;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace APIRelatorios.Testes.Application.Features.Commands.Rota;

public class RotaHandlersTests
{
    [Fact]
    public async Task CreateRota_DeveCriarRotaComFiscaisValidados()
    {
        var commands = new Mock<IRotaCommands>();
        var ids = new Mock<IValidateIds>();
        ids.Setup(x => x.UserExisteAsync(10)).ReturnsAsync(true);
        var handler = new CreateRotaHandler(commands.Object, Mock.Of<IUserQuery>(), ids.Object);
        var id = Guid.NewGuid();

        await handler.Handle(new CreateRotaCommand(id, "Rota", Concessionarias.ENEL,
            "AL-1", [10], TipoFiscalizacao.Alimentadores), default);

        commands.Verify(x => x.CreateRotaAsync(It.Is<APIRelatorios.Dommain.Entities.Rota>(rota =>
            rota.RotaId == id && rota.Fiscais.Single().UserId == 10)), Times.Once);
    }

    [Fact]
    public async Task AddFiscal_DevePersistirCadaFiscal()
    {
        var query = new Mock<IRotaQuery>();
        var commands = new Mock<IRotaCommands>();
        var ids = new Mock<IValidateIds>();
        var rotaId = Guid.NewGuid();
        query.Setup(x => x.BuscarRotaID(rotaId)).ReturnsAsync(CreateRota(rotaId));
        ids.Setup(x => x.UserExisteAsync(It.IsAny<int>())).ReturnsAsync(true);
        var handler = new AddFiscalRotaHandler(query.Object, commands.Object, ids.Object,
            Mock.Of<ILogger<AddFiscalRotaHandler>>());

        await handler.Handle(new AddFiscalRotaCommand(rotaId, [1, 2]), default);

        commands.Verify(x => x.AdicionarFiscalRota(It.IsAny<UsuarioRota>()), Times.Exactly(2));
    }

    [Fact]
    public async Task RemoveFiscal_DeveRemoverCadaFiscal()
    {
        var query = new Mock<IRotaQuery>();
        var commands = new Mock<IRotaCommands>();
        var ids = new Mock<IValidateIds>();
        var rotaId = Guid.NewGuid();
        query.Setup(x => x.BuscarRotaID(rotaId)).ReturnsAsync(CreateRota(rotaId));
        ids.Setup(x => x.UserExisteAsync(It.IsAny<int>())).ReturnsAsync(true);
        var handler = new RemoveFiscalRotaHandler(commands.Object, query.Object, ids.Object);

        await handler.Handle(new RemoveFiscalRotaCommand(rotaId, [1, 2]), default);

        commands.Verify(x => x.RemoverFiscalRota(It.IsAny<int>(), rotaId), Times.Exactly(2));
    }

    [Fact]
    public async Task UpdateNome_DeveAtualizarEntidadeEPersistir()
    {
        var query = new Mock<IRotaQuery>();
        var commands = new Mock<IRotaCommands>();
        var rotaId = Guid.NewGuid();
        var rota = CreateRota(rotaId);
        query.Setup(x => x.BuscarRotaID(rotaId)).ReturnsAsync(rota);
        var handler = new UpdateNomeRotaHandler(commands.Object, query.Object, Mock.Of<IValidateIds>());

        await handler.Handle(new UpdateNomeRotaCommand(rotaId, "Nova rota", "AL-2"), default);

        Assert.Equal("Nova rota", rota.NomeRota);
        Assert.Equal("AL-2", rota.Alimentador);
        commands.Verify(x => x.UpdateRotaAsync(rota), Times.Once);
    }

    [Fact]
    public async Task DeleteRota_DeveInativarEPersistir()
    {
        var query = new Mock<IRotaQuery>();
        var commands = new Mock<IRotaCommands>();
        var rotaId = Guid.NewGuid();
        var rota = CreateRota(rotaId);
        query.Setup(x => x.BuscarRotaID(rotaId)).ReturnsAsync(rota);
        var handler = new DeleteRotaHandler(commands.Object, query.Object, Mock.Of<IValidateIds>());

        await handler.Handle(new DeleteRotaCommand(rotaId), default);

        Assert.False(rota.isValid);
        commands.Verify(x => x.DeleteRotaAsync(rota), Times.Once);
    }

    private static APIRelatorios.Dommain.Entities.Rota CreateRota(Guid id) => new(id, "Rota", Concessionarias.ENEL,
        "AL-1", DateTime.UtcNow, TipoFiscalizacao.Alimentadores);
}
