using APIRelatorios.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace APIRelatorios.WebAPI.Exceptions;

public class ArgumentNullExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken
        )
    {
        if (exception is not ArgumentNullException argumentNull)
            return false;

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsJsonAsync(new
        {
            errorCode = "NULL_REFERENCE",
            message = argumentNull.Message
        }, cancellationToken);

        return true;
    }
}
