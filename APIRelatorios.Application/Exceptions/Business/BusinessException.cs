namespace APIRelatorios.Application.Exceptions.Business;

public class BusinessException : AppException
{
    protected BusinessException(
        string errorCode,
        string message)
        : base(errorCode, message)
    {
    }
}
