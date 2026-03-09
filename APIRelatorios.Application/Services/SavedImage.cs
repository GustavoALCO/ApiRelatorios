using Azure.Storage.Blobs;
using ChatApplication.Application.Interfaces;
using ChatApplication.Application.Settings;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace ChatApplication.Application.Service;

public class SavedImage : ISavedImages
{
    private readonly BlobSettings _blobService;

    public SavedImage(IOptions<BlobSettings> configuration)
    {
        _blobService = configuration.Value;
    }

    public async Task<string> UploadBase64ImagesAsync(
    string alimentador,
    string fiscal,
    string horario,
    string base64Image,
    string container)
    {
        // 1️⃣ Extrai o tipo da imagem do base64
        var match = Regex.Match(base64Image, @"(?<type>[a-zA-Z0-9]+);base64,");
            
        if (!match.Success)
            throw new InvalidOperationException("Formato de imagem inválido service");

        var imageType = match.Groups["type"].Value.ToLower();

        // 2️⃣ Define extensão e content type
        string extension = imageType switch
        {
            "png" => ".png",
            "jpeg" => ".jpg",
            "jpg" => ".jpg",
            "gif" => ".gif",
            _ => throw new InvalidOperationException("Formato não suportado")
        };

        string contentType = $"image/{imageType}";

        var fileName = $"{alimentador}_{fiscal}_{horario}{extension}";

        // 3️⃣ Remove prefixo do base64
        var data = Regex.Replace(base64Image, @"^data:image\/[a-zA-Z0-9]+;base64,", "");

        byte[] imageBytes = Convert.FromBase64String(data);

        var blobClient = new BlobClient(_blobService.ConnectionString, container, fileName) ?? throw new Exception("Não foi possivel enviar imagem para o blob");

        using (var stream = new MemoryStream(imageBytes))
        {
            await blobClient.UploadAsync(stream, new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = contentType
            });
        }

        return blobClient.Uri.AbsoluteUri;
    }

    public async Task DeleteImagesAsync(string imageNames, int indiceContainer)
    {
        var containerClient = new BlobContainerClient(_blobService.ConnectionString, _blobService.Container);
        //Conecta no blob passando o nome do container

        var imageName = GetBlobNameFromUrl(imageNames);
        //Separa apenas o id da imagem para ser apagada 

        var blobClient = containerClient.GetBlobClient(imageName);
        //Passa o Id da imagem a ser excluida

        if (await blobClient.ExistsAsync())
        {
            await blobClient.DeleteAsync();
            Console.WriteLine($"Imagem {imageName} deletada com sucesso.");
        }//Se acha ele direciona para o deleteAsync onnde vai ser apagado
        else
        {
            Console.WriteLine($"Imagem {imageName} não encontrada.");
        }//Se não achar ele envia uma mensagem no console informando que não foi achado

    }

    private static string GetBlobNameFromUrl(string url)
    {
        Uri uri = new(url);
        return Path.GetFileName(uri.LocalPath);
    }
}