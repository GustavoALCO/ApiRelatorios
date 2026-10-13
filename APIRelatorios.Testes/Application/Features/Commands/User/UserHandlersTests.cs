using APIRelatorios.Application.Features.Commands.User;
using APIRelatorios.Application.Features.Commands.User.Handlers;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Services;
using APIRelatorios.Dommain.Interfaces.User;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using UserEntity = APIRelatorios.Dommain.Entities.User;

namespace APIRelatorios.Testes.Application.Features.Commands.User;

public class UserHandlersTests
{
    [Fact]
    public async Task CreateUser_DeveGerarLoginHashESalvarUsuario()
    {
        var query = new Mock<IUserQuery>();
        var commands = new Mock<IUserCommands>();
        var hasher = new Mock<IPasswordHasher>();
        query.Setup(x => x.BuscarTodosFiscalLogin("maria.silva"))
            .ReturnsAsync(new List<UserEntity>());
        hasher.Setup(x => x.GenerateHash(It.IsAny<int>())).Returns([1, 2]);
        hasher.Setup(x => x.HashPassword("senha", It.IsAny<byte[]>())).Returns("hash");
        var handler = new CreateUserHandler(query.Object, commands.Object, hasher.Object,
            Mock.Of<ILogger<CreateUserHandler>>());

        await handler.Handle(new CreateUsuarioCommand("Maria", "Silva", "senha", false), default);

        commands.Verify(x => x.CreateUser(It.Is<UserEntity>(user =>
            user.Login == "maria.silva" && user.HashPassword == "hash" && !user.IsAdmin)), Times.Once);
    }

    [Fact]
    public async Task Login_DeveRetornarToken_QuandoSenhaForValida()
    {
        var query = new Mock<IUserQuery>();
        var token = new Mock<IJwtTokenService>();
        var hasher = new Mock<IPasswordHasher>();
        var user = CreateUser();
        query.Setup(x => x.BuscarFiscalLogin("maria.silva")).ReturnsAsync(user);
        hasher.Setup(x => x.VerifyPassword("senha", user.HashPassword, user.Salt)).Returns(true);
        token.Setup(x => x.GenerateToken(user.UserId, user.Login, user.IsAdmin)).Returns("jwt");
        var handler = new LoginHandler(query.Object, token.Object, hasher.Object, Mock.Of<ILogger<LoginHandler>>());

        var result = await handler.Handle(new LoginCommandsCommand("maria.silva", "senha"), default);

        Assert.Equal("jwt", result);
    }

    [Fact]
    public async Task UpdatePassword_DeveAtualizarHashESalvarUsuario()
    {
        var query = new Mock<IUserQuery>();
        var commands = new Mock<IUserCommands>();
        var hasher = new Mock<IPasswordHasher>();
        var user = CreateUser();
        query.Setup(x => x.BuscarFiscalId(1)).ReturnsAsync(user);
        hasher.Setup(x => x.HashPassword("nova-senha", user.Salt)).Returns("novo-hash");
        var handler = new UpdatePasswordHandler(commands.Object, hasher.Object, query.Object,
            Mock.Of<ILogger<UpdatePasswordHandler>>());

        await handler.Handle(new UpdatePasswordCommand(1, "nova-senha"), default);

        Assert.Equal("novo-hash", user.HashPassword);
        commands.Verify(x => x.UpdateUser(user), Times.Once);
    }

    [Fact]
    public async Task UpdateUsuario_DeveAlterarCamposInformadosESalvar()
    {
        var query = new Mock<IUserQuery>();
        var commands = new Mock<IUserCommands>();
        var user = CreateUser();
        query.Setup(x => x.BuscarFiscalId(1)).ReturnsAsync(user);
        var handler = new UpdateUsuarioHandler(query.Object, commands.Object, Mock.Of<IValidateIds>(),
            Mock.Of<ILogger<UpdateUsuarioHandler>>());

        await handler.Handle(new AlterarUsuarioCommand(1, "novo.login", "Ana", "Souza", true), default);

        Assert.Equal("novo.login", user.Login);
        Assert.Equal("Ana", user.Name);
        Assert.Equal("Souza", user.LastName);
        Assert.True(user.IsAdmin);
        commands.Verify(x => x.UpdateUser(user), Times.Once);
    }

    [Fact]
    public async Task DeleteUsuario_DeveInativarESalvarUsuario()
    {
        var query = new Mock<IUserQuery>();
        var commands = new Mock<IUserCommands>();
        var user = CreateUser();
        query.Setup(x => x.BuscarFiscalId(1)).ReturnsAsync(user);
        var handler = new DeleteUsuarioHandler(query.Object, commands.Object, Mock.Of<IValidateIds>(),
            Mock.Of<ILogger<DeleteUsuarioHandler>>());

        await handler.Handle(new DeleteUsuarioCommand(1), default);

        Assert.True(user.IsValid);
        commands.Verify(x => x.DeleteUser(user), Times.Once);
    }

    private static UserEntity CreateUser()
    {
        var user = new UserEntity();
        user.CreateUser("maria.silva", "Maria", "Silva", "hash", [1, 2], false);
        return user;
    }
}
