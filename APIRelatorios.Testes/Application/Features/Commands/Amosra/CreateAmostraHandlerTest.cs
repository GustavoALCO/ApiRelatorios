using APIRelatorios.Application.Features.Commands.Amostra;
using APIRelatorios.Application.Features.Commands.Amostra.Handler;
using APIRelatorios.Domain.Interfaces.Amostra;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace APIRelatorios.Testes.Application.Features.Commands.Amostra;

public class CreateAmostraHandlerTests
{
    private readonly Mock<IAmostraCommands> _commandsMock;
    private readonly Mock<ILogger<CreateAmostraHandler>> _loggerMock;
    private readonly CreateAmostraHandler _handler;

    public CreateAmostraHandlerTests()
    {
        _commandsMock = new Mock<IAmostraCommands>();
        _loggerMock = new Mock<ILogger<CreateAmostraHandler>>();

        _handler = new CreateAmostraHandler(
            _commandsMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_DeveSalvarAmostra_QuandoCommandForValido()
    {
        // Arrange
        var command = new CreateAmostraCommand(
            Guid.NewGuid(),
            "SEQ001",
            "SEQ002",
            "1000.00",
            "Descrição TUC",
            "Descrição Técnica",
            "ODI Engenharia",
            "Instalação",
            "Endereço",
            "Município",
            0.0,
            0.0,
            "TUC1",
            "TUC2",
            "TUC3",
            "TUC4",
            "TUC5",
            "TUC6",
            "NumSerie",
            "PosicaoOperativa",
            "Equipamento"
        );

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _commandsMock.Verify(
            x => x.SaveAmostra(It.IsAny<Domain.Entities.Amostra>()),
            Times.Once
        );
    }
}