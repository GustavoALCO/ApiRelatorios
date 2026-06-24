using APIRelatorios.Application.Exceptions.InternalError;

namespace APIRelatorios.Application.Exceptions.Business;

public sealed class Base64Exception : InternalErrorException
{
    public Base64Exception() : 
        base(
        ErrorCodes.Base64Error, 
        "Erro ao tratar o Base64"
        )
    {
    }
}
