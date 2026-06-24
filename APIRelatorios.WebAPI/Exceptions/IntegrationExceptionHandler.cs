using APIRelatorios.Application.Exceptions.Business;
using APIRelatorios.Application.Exceptions.Integrations;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Exceptions;

public class IntegrationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
                                                Exception exception, 
                                                CancellationToken cancellationToken
                                                )
    {
        if (exception is not IntegrationException integrationException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status503ServiceUnavailable,
            Title = "Serviço Indisponivel",
            Detail = integrationException.Message,
            Instance = httpContext.Request.Path
        };

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}
