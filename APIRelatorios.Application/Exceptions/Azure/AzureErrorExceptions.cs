namespace APIRelatorios.Application.Exceptions.Azure;

public class AzureErrorExceptions : AppException
{
    protected AzureErrorExceptions(
        string errorCode,
        string message)
        : base(errorCode, message)
    {
    }
}
