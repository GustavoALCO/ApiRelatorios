using APIRelatorios.Dommain.Entities;
using Azure.Storage.Blobs;
using ChatApplication.Application.Interfaces;
using ChatApplication.Application.Settings;
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

    /// Limpa o Base64 removendo prefixos e caracteres inválidos
    private static string CleanBase64(string base64)
    {
        var cleaned = Regex.Replace(base64, @"^data:image\/[a-zA-Z0-9]+;base64,", "");
        cleaned = cleaned.Replace("\r", "").Replace("\n", "").Replace(" ", "");
        return cleaned;
    }

    public async Task<string> UploadBase64ImagesAsync(
        string alimentador,
        string fiscal,
        string horario,
        string base64Image,
        string container,
        string qualidade,
        int index)
    {
        // Sempre JPEG
        string extension = ".jpg";
        string contentType = "image/jpeg";
        var fileName = $"{alimentador.Trim()}_{fiscal}_{horario}_{index}_{qualidade}_{Guid.NewGuid()}{extension}";

        // Limpa e converte Base64
        var cleanedBase64 = CleanBase64(base64Image);
        byte[] imageBytes;
        try
        {
            imageBytes = Convert.FromBase64String(cleanedBase64);
        }
        catch (FormatException)
        {
            throw new InvalidOperationException("O Base64 da imagem está em formato inválido.");
        }

        var blobClient = new BlobClient(_blobService.ConnectionString, container, fileName)
                         ?? throw new Exception("Não foi possível enviar imagem para o blob");

        using var stream = new MemoryStream(imageBytes);
        await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobHttpHeaders
        {
            ContentType = contentType
        });

        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<List<ImageData>> UploadListBase64ImagesAsync(
        string alimentador,
        string fiscal,
        string horario,
        List<string> base64Images,
        string container,
        Guid evidenciaId)
    {
        List<ImageData> images = new();
        int index = 0;

        foreach (var base64 in base64Images)
        {
            index++;
            var cleanedBase64 = CleanBase64(base64);
            byte[] originalBytes = Convert.FromBase64String(cleanedBase64);

            byte[] imageLow = RedimensionarImagem(originalBytes, 300, 60);
            byte[] imageMedium = RedimensionarImagem(originalBytes, 800, 75);

            // Faz upload das três versões
            var urlLow = await UploadBase64ImagesAsync(alimentador, fiscal, horario, Convert.ToBase64String(imageLow), container, "low", index);
            var urlMedium = await UploadBase64ImagesAsync(alimentador, fiscal, horario, Convert.ToBase64String(imageMedium), container, "medium", index);
            var urlOriginal = await UploadBase64ImagesAsync(alimentador, fiscal, horario, Convert.ToBase64String(originalBytes), container, "original", index);

            images.Add(new ImageData(urlOriginal, urlMedium, urlLow, evidenciaId));
        }

        return images;
    }

    public async Task DeleteImagesAsync(string imageUrl, int indiceContainer)
    {
        var containerClient = new BlobContainerClient(_blobService.ConnectionString, _blobService.Container);
        var imageName = GetBlobNameFromUrl(imageUrl);
        var blobClient = containerClient.GetBlobClient(imageName);

        if (await blobClient.ExistsAsync())
        {
            await blobClient.DeleteAsync();
            Console.WriteLine($"Imagem {imageName} deletada com sucesso.");
        }
        else
        {
            Console.WriteLine($"Imagem {imageName} não encontrada.");
        }
    }

    public byte[] RedimensionarImagem(byte[] imageBytes, int largura, int qualidade)
    {
        using var inputStream = new MemoryStream(imageBytes);
        using var original = SKBitmap.Decode(inputStream);

        if (original == null)
            throw new Exception("Erro ao decodificar imagem");

        float proporcao = (float)largura / original.Width;
        int altura = (int)(original.Height * proporcao);

        using var resized = original.Resize(new SKImageInfo(largura, altura), SKFilterQuality.High);
        using var image = SKImage.FromBitmap(resized);
        using var data = image.Encode(SKEncodedImageFormat.Jpeg, qualidade);

        return data.ToArray();
    }

    private static string GetBlobNameFromUrl(string url)
    {
        Uri uri = new(url);
        return Path.GetFileName(uri.LocalPath);
    }
}