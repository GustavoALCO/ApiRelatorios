using APIRelatorios.Application.Exceptions.Authentication;
using APIRelatorios.Application.Exceptions.Azure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace APIRelatorios.WebAPI.Exceptions;

public class AuthenticationExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
                                            Exception exception,
                                            CancellationToken cancellationToken
                                         )
    {

        if (exception is not AuthenticationException authenticationException)
        {
            return false;
        }

        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Falha ao Autenticar",
            Detail = authenticationException.Message,
            Instance = httpContext.Request.Path
        };

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}
