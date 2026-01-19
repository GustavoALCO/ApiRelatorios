using APIRelatorios.Dommain.Entities;

namespace APIRelatorios.Dommain.Interfaces.User;

public interface IUserCommands
{
    Task CreateUser(Entities.User user);

    Task DeleteUser(Entities.User user);

    Task UpdateUser(Entities.User user);
}
