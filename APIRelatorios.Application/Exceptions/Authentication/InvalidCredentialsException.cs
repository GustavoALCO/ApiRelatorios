namespace APIRelatorios.Application.Exceptions.Authentication;

public sealed class InvalidCredentialsException : AuthenticationException
{
    public InvalidCredentialsException() :
        base(
        ErrorCodes.InvalidCredentials, 
        "Login ou Senha Incorretos")
    {
    }
}
