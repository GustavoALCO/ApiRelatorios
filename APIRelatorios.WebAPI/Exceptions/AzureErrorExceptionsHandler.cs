using APIRelatorios.Application.Exceptions.Azure;
using APIRelatorios.Application.Exceptions.NotFound;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Exceptions;

public class AzureErrorExceptionsHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
                                            Exception exception, 
                                            CancellationToken cancellationToken
                                         )
    {
        
        if ( exception is not AzureErrorExceptions azureErrorExceptions)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status503ServiceUnavailable,
            Title = "Falha na comunicação com os serviços Azure",
            Detail = azureErrorExceptions.Message,
            Instance = httpContext.Request.Path
        };

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}
