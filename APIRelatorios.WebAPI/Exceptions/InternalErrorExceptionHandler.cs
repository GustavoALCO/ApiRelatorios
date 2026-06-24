using APIRelatorios.Application.Exceptions.Integrations;
using APIRelatorios.Application.Exceptions.InternalError;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Exceptions;

public class InternalErrorExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
                                                Exception exception, 
                                                CancellationToken cancellationToken
                                                )
    {
        if (exception is not InternalErrorException internalErrorException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Erro Interno",
            Detail = internalErrorException.Message,
            Instance = httpContext.Request.Path
        };

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}
