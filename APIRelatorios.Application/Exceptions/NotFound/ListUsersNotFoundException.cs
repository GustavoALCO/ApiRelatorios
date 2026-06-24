namespace APIRelatorios.Application.Exceptions.NotFound;

public sealed class ListUsersNotFoundException : NotFoundException
{
    public ListUsersNotFoundException() : 
        base(
        ErrorCodes.UserNotFound, 
        "Lista de Fiscais não é Valida. Verifique se todos estão validos"
        )
    {
    }
}
