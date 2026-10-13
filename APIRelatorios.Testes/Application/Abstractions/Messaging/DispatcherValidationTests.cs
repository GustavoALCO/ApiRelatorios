using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Features.Commands.User;
using APIRelatorios.Application.Validations.User;
using FluentValidation;
using Moq;
using Xunit;

namespace APIRelatorios.Testes.Application.Abstractions.Messaging;

public class DispatcherValidationTests
{
    [Fact]
    public async Task Send_DeveInterromperFluxo_QuandoComandoForInvalido()
    {
        var provider = new Mock<IServiceProvider>();
        var validator = new LoginValidate();
        provider.Setup(x => x.GetService(typeof(IValidator<LoginCommandsCommand>)))
            .Returns(validator);
        var dispatcher = new Dispatcher(provider.Object);

        await Assert.ThrowsAsync<ValidationException>(() =>
            dispatcher.Send<LoginCommandsCommand, string>(
                new LoginCommandsCommand("", "senha")));

        provider.Verify(x => x.GetService(typeof(ICommandHandler<LoginCommandsCommand, string>)), Times.Never);
    }
}
