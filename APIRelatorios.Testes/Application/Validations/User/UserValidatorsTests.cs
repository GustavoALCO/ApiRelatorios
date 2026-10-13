using APIRelatorios.Application.Features.Commands.User;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Application.Validations.User;
using Moq;
using Xunit;

namespace APIRelatorios.Testes.Application.Validations.User;

public class UserValidatorsTests
{
    private readonly IValidateIds _validateIds = Mock.Of<IValidateIds>();

    [Fact]
    public void CreateUsuario_DeveAceitarDadosObrigatorios()
    {
        var result = new CreateUsuarioValidate().Validate(
            new CreateUsuarioCommand("Maria", "Silva", "senha-segura", false));

        Assert.True(result.IsValid, result.ToString());
    }

    [Theory]
    [InlineData("", "senha")]
    [InlineData("Maria", "")]
    public void CreateUsuario_DeveRejeitarNomeOuSenhaVazios(string nome, string senha)
    {
        var result = new CreateUsuarioValidate().Validate(
            new CreateUsuarioCommand(nome, "Silva", senha, false));

        Assert.False(result.IsValid);
    }

    [Theory]
    [InlineData("", "senha")]
    [InlineData("maria", "")]
    public void Login_DeveRejeitarCredenciaisVazias(string login, string senha)
    {
        var result = new LoginValidate().Validate(new LoginCommandsCommand(login, senha));

        Assert.False(result.IsValid);
    }

    [Fact]
    public void Login_DeveAceitarCredenciaisPreenchidas()
    {
        var result = new LoginValidate().Validate(new LoginCommandsCommand("maria", "senha"));

        Assert.True(result.IsValid, result.ToString());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void AlterarUsuario_DeveRejeitarIdInvalido(int id)
    {
        var result = new AlterarUsuarioValidate(_validateIds).Validate(
            new AlterarUsuarioCommand(id, null, null, null, null));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.PropertyName == "userId");
    }

    [Fact]
    public void DeleteUsuario_DeveAceitarIdPositivo()
    {
        var result = new DeleteUsuarioValidate(_validateIds).Validate(new DeleteUsuarioCommand(1));

        Assert.True(result.IsValid, result.ToString());
    }
}
