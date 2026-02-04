using APIRelatorios.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Services;

public class ValidateBase64 : IValidateBase64
{
    public bool IsValidBase64String(string base64String)
    {
        if (string.IsNullOrWhiteSpace(base64String))
            return false;

        var base64 = base64String.Trim();

        // Remove data:image/...;base64,
        var commaIndex = base64.IndexOf(',');
        if (commaIndex >= 0)
            base64 = base64[(commaIndex + 1)..];

        base64 = base64
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace(" ", "");

        Span<byte> buffer = stackalloc byte[base64.Length];
        return Convert.TryFromBase64String(base64, buffer, out _);
    }
}
