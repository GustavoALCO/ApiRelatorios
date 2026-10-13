using APIRelatorios.Application.Features.Queries.User;
using APIRelatorios.Application.Features.Querys.User.Handler;
using APIRelatorios.Dommain.Interfaces.User;
using Moq;
using Xunit;
using UserEntity = APIRelatorios.Dommain.Entities.User;

namespace APIRelatorios.Testes.Application.Features.Queries.User;

public class BuscarTodosUsuariosHandlerTests
{
    [Fact]
    public async Task Handle_DeveMapearUsuariosParaDto()
    {
        var user = new UserEntity();
        user.CreateUser("maria.silva", "Maria", "Silva", "hash", [1], true);
        var query = new Mock<IUserQuery>();
        query.Setup(x => x.BuscarTodosFiscais()).ReturnsAsync(new List<UserEntity> { user });
        var handler = new BuscarTodosUsuariosHandler(query.Object);

        var result = await handler.Handle(new BuscarTodosusuariosQuery(), default);

        var dto = Assert.Single(result);
        Assert.Equal("Maria", dto.Name);
        Assert.Equal("Silva", dto.Lastname);
        Assert.True(dto.IsAdmin);
    }
}
