using APIRelatorios.Application.Exceptions.Business;
using APIRelatorios.Dommain.Entities;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
        var cleaned = Regex.Replace(
            base64,
            @"^data:image\/[a-zA-Z0-9]+;base64,",
            ""
        );

        return cleaned
            .Replace("\r", "")
            .Replace("\n", "")
            .Replace(" ", "");
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

        var fileName =
            $"{safeAlimentador}_{safeFiscal}_{safeHorario}_{pluscode}_{qualidade}_{index}_{Guid.NewGuid()}.jpg";

        var cleanedBase64 = CleanBase64(base64Image);
        var imageBytes = Convert.FromBase64String(cleanedBase64);

        var blobClient = new BlobClient(
            _blobService.ConnectionString,
            container,
            fileName
        );

        using var stream = new MemoryStream(imageBytes);

        await blobClient.UploadAsync(
            stream,
            new BlobHttpHeaders
            {
                ContentType = "image/jpeg"
            }
        );

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

            byte[] originalBytes =
                Convert.FromBase64String(cleanedBase64);

            byte[] imageWithText = AdicionaTexto(
                originalBytes,
                alimentador,
                $"Lat: {lat}, Long: {log}",
                rua,
                horario
            );

            byte[] imageLow = RedimensionarImagem(
                imageWithText,
                300,
                100
            );

            var urlLow = await UploadBase64ImagesAsync(
                alimentador,
                fiscal,
                horario,
                plusCode,
                Convert.ToBase64String(imageLow),
                container,
                "low",
                index
            );

            var urlOriginal = await UploadBase64ImagesAsync(
                alimentador,
                fiscal,
                horario,
                plusCode,
                Convert.ToBase64String(imageWithText),
                container,
                "original",
                index
            );

            images.Add(
                new ImageData(
                    urlOriginal,
                    urlLow,
                    evidenciaId
                )
            );
        }

        return images;
    }

    public byte[] RedimensionarImagem(
        byte[] imageBytes,
        int largura,
        int qualidade)
    {
        using var inputStream = new MemoryStream(imageBytes);
        using var bitmap = SKBitmap.Decode(inputStream);

        if (bitmap == null)
            throw new Base64Exception();

        float proporcao = (float)largura / bitmap.Width;

        int altura = (int)(bitmap.Height * proporcao);

        using var resized = bitmap.Resize(
            new SKImageInfo(largura, altura),
            new SKSamplingOptions(
                SKFilterMode.Linear,
                SKMipmapMode.Linear
            )
        );

        if (resized == null)
            throw new Base64Exception();

        using var image = SKImage.FromBitmap(resized);

        using var data = image.Encode(
            SKEncodedImageFormat.Jpeg,
            qualidade
        );

        return data.ToArray();
    }

    private byte[] AdicionaTexto(
        byte[] imageBytes,
        string linha1,
        string linha2,
        string linha3,
        string linha4)
    {
        using var inputStream = new MemoryStream(imageBytes);
        using var original = SKBitmap.Decode(inputStream);

        if (original == null)
            throw new Base64Exception();

        using var surface = SKSurface.Create(
            new SKImageInfo(
                original.Width,
                original.Height
            )
        );

        var canvas = surface.Canvas;

        canvas.DrawBitmap(original, 0, 0);

        var linhas = new[]
        {
            linha1,
            linha2,
            linha3,
            linha4
        }
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .ToArray();

        if (linhas.Length == 0)
        {
            using var imageSemTexto = surface.Snapshot();

            using var dataSemTexto = imageSemTexto.Encode(
                SKEncodedImageFormat.Jpeg,
                90
            );

            return dataSemTexto.ToArray();
        }

        float menorLado = Math.Min(
            original.Width,
            original.Height
        );

        float textSize = menorLado * 0.04f;
        float margem = menorLado * 0.025f;
        float lineHeight = textSize * 1.35f;

        float x = margem;

        float yBase =
            original.Height -
            margem -
            (lineHeight * (linhas.Length - 1));

        using var font = new SKFont
        {
            Size = textSize
        };

        using var paint = new SKPaint
        {
            Color = SKColors.Yellow,
            IsAntialias = true
        };

        using var shadow = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true
        };

        for (int i = 0; i < linhas.Length; i++)
        {
            float y = yBase + (i * lineHeight);

            canvas.DrawText(
                linhas[i],
                x + 2,
                y + 2,
                SKTextAlign.Left,
                font,
                shadow
            );

            canvas.DrawText(
                linhas[i],
                x,
                y,
                SKTextAlign.Left,
                font,
                paint
            );
        }

        using var image = surface.Snapshot();

        using var data = image.Encode(
            SKEncodedImageFormat.Jpeg,
            90
        );

        return data.ToArray();
    }

    public async Task<List<string>> UploadListImagensAmostra(string seqIsa, string fiscal, List<string> base64Images, string container, double lat, double log)
    {
        var imagesUrl = new List<string>();

        int index = 0;

        string plusCode = OpenLocationCode.Encode(lat, log, 11);

        foreach (var image in base64Images)
        {
            index++;

            string url = await UploadBase64ImagesAsync(
                seqIsa,
                fiscal,
                DateTime.Now.ToString("yyyyMMdd_HHmmss"),
                plusCode,
                image,
                container,
                "compact",
                index
            );

            imagesUrl.Add(url);
        }

        return imagesUrl;
    }
}