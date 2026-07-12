using APIRelatorios.Application.Exceptions.NotFound;
using Microsoft.AspNetCore.Diagnostics;

namespace APIRelatorios.WebAPI.Exceptions;

public class AmostraNullExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken
        )
    {
        if (exception is not AmostraNotFoundException amostraNotFound)
            return false;

        httpContext.Response.StatusCode = StatusCodes.Status404NotFound;

        await httpContext.Response.WriteAsJsonAsync(new
        {
            errorCode = amostraNotFound.ErrorCode,
            message = amostraNotFound.Message
        }, cancellationToken);

        return true;
    }
}
