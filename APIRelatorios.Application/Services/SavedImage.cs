using APIRelatorios.Application.Exceptions.Business;
using APIRelatorios.Dommain.Entities;
using Azure.Storage.Blobs;
using ChatApplication.Application.Interfaces;
using ChatApplication.Application.Settings;
using Google.OpenLocationCode;
using Microsoft.Extensions.Options;
using SkiaSharp;
using System.Text.RegularExpressions;

namespace ChatApplication.Application.Service;

public class SavedImage : ISavedImages
{
    private readonly BlobSettings _blobService;

    public SavedImage(IOptions<BlobSettings> configuration)
    {
        _blobService = configuration.Value;
    }

    private static string CleanBase64(string base64)
    {
        var cleaned = Regex.Replace(base64, @"^data:image\/[a-zA-Z0-9]+;base64,", "");
        return cleaned.Replace("\r", "").Replace("\n", "").Replace(" ", "");
    }

    private static string Sanitize(string input)
    {
        return Regex.Replace(input, @"[^a-zA-Z0-9_\-]", "_");
    }

    public async Task<string> UploadBase64ImagesAsync(
        string alimentador,
        string fiscal,
        string horario,
        string pluscode,
        string base64Image,
        string container,
        string qualidade,
        int index)
    {
        var safeHorario = Sanitize(horario);
        var safeAlimentador = Sanitize(alimentador);
        var safeFiscal = Sanitize(fiscal);

        var fileName = $"{safeAlimentador}_{safeFiscal}_{safeHorario}_{pluscode}_{qualidade}_{index}_{Guid.NewGuid()}.jpg";

        var cleanedBase64 = CleanBase64(base64Image);
        var imageBytes = Convert.FromBase64String(cleanedBase64);

        var blobClient = new BlobClient(_blobService.ConnectionString, container, fileName);

        using var stream = new MemoryStream(imageBytes);

        await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobHttpHeaders
        {
            ContentType = "image/jpeg"
        });

        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<List<ImageData>> UploadListBase64ImagesAsync(
        string alimentador,
        string fiscal,
        string horario,
        List<string> base64Images,
        string container,
        string rua,
        Guid evidenciaId,
        double lat,
        double log)
    {
        List<ImageData> images = new();
        int index = 0;

        string plusCode = OpenLocationCode.Encode(lat, log, 11);

        foreach (var base64 in base64Images)
        {
            index++;

            var cleanedBase64 = CleanBase64(base64);
            byte[] originalBytes = Convert.FromBase64String(cleanedBase64);

            byte[] imageWithText = AdicionaTexto(
                originalBytes,
                alimentador,
                $"Lat: {lat}, Long: {log}",
                rua,
                horario
            );

            byte[] imageLow = RedimensionarImagem(imageWithText, 300, 100);

            var urlLow = await UploadBase64ImagesAsync(
                alimentador, fiscal, horario, plusCode,
                Convert.ToBase64String(imageLow), container, "low", index);

            var urlOriginal = await UploadBase64ImagesAsync(
                alimentador, fiscal, horario, plusCode,
                Convert.ToBase64String(imageWithText), container, "original", index);

            images.Add(new ImageData(urlOriginal, urlLow, evidenciaId));
        }

        return images;
    }

    public byte[] RedimensionarImagem(byte[] imageBytes, int largura, int qualidade)
    {
        using var inputStream = new MemoryStream(imageBytes);
        using var bitmap = SKBitmap.Decode(inputStream);

        if (bitmap == null)
            throw new Base64Exception();

        float proporcao = (float)largura / bitmap.Width;
        int altura = (int)(bitmap.Height * proporcao);

        using var resized = bitmap.Resize(
            new SKImageInfo(largura, altura),
            new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear)
        );

        using var image = SKImage.FromBitmap(resized);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, qualidade);

        return data.ToArray();
    }
    private byte[] AdicionaTexto(byte[] imageBytes, string linha1, string linha2, string linha3,string linha4)
    {
        using var inputStream = new MemoryStream(imageBytes);
        using var original = SKBitmap.Decode(inputStream);

        if (original == null)
            throw new Base64Exception();

        using var surface = SKSurface.Create(new SKImageInfo(original.Width, original.Height));
        var canvas = surface.Canvas;

        canvas.DrawBitmap(original, 0, 0);

        float textSize = original.Width * 0.04f;

        using var paint = new SKPaint
        {
            Color = SKColors.Yellow,
            IsAntialias = true
        };

        using var shadow = new SKPaint
        {
            Color = SKColors.Black,
            StrokeWidth = 0.02f,
            IsAntialias = true
        };

        using var font = new SKFont { Size = textSize };

        float x = 20;
        float lineHeight = textSize + 10;

        string[] linhas = { linha1, linha2, linha3, linha4 };

        float yBase = original.Height - (lineHeight * linhas.Length);

        for (int i = 0; i < linhas.Length; i++)
        {
            float y = yBase + (i * lineHeight);

            canvas.DrawText(linhas[i], x + 2, y + 2, SKTextAlign.Left, font, shadow);
            canvas.DrawText(linhas[i], x, y, SKTextAlign.Left, font, paint);
        }

        using var image = surface.Snapshot();
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, 90);

        return data.ToArray();
    }
}