namespace APIRelatorios.Application.Exceptions.NotFound;

public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(int id)
        : base(
            ErrorCodes.UserNotFound,
            $"Usuário com ID '{id}' não foi encontrado.")
    {
    }
}