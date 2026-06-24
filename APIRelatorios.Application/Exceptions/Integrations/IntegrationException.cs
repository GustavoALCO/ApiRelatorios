namespace APIRelatorios.Application.Exceptions.Integrations;

public class IntegrationException : AppException
{
    protected IntegrationException(
        string errorCode,
        string message)
        : base(errorCode, message)
    {
    }
}
