using APIRelatorios.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

public class AppExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not AppException appException)
            return false;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(new
        {
            errorCode = appException.ErrorCode,
            message = appException.Message
        }, cancellationToken);

        return true;
    }
}