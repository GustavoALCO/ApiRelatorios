namespace APIRelatorios.Application.Exceptions.Authentication;

public class AuthenticationException : AppException
{
    protected AuthenticationException(
        string errorCode,
        string message)
        : base(errorCode, message)
    {
    }
}
