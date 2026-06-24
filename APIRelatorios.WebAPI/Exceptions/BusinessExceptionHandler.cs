using APIRelatorios.Application.Exceptions.Azure;
using APIRelatorios.Application.Exceptions.Business;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Exceptions;

public class BusinessExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
                                          Exception exception, 
                                          CancellationToken cancellationToken
                                            )
    {
        
        if (exception is not BusinessException businessException) 
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = "Conflito de regra de negócio",
            Detail = businessException.Message,
            Instance = httpContext.Request.Path
        };

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}
