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

    public async Task<string> UploadBase64ImagesAsync(string base64Image, string container)
    {
        var fileName = $"{Guid.NewGuid().ToString()}.jpg";
        // Gera um nome único para a imagem

        var data = new Regex(@"^data:image\/[a-z]+;base64,").Replace(base64Image, "");
        // Remove a parte desnecessária do base64

        byte[] imageBytes = Convert.FromBase64String(data);
        // Converte a string base64 em um array de bytes

        var blobClient = new BlobClient(_blobService.ConnectionString, container, fileName);
        // Cria um cliente para o Blob Storage

        // Envia a imagem para o Storage de forma assíncrona
        using (var stream = new MemoryStream(imageBytes))
        {
            await blobClient.UploadAsync(stream);
        }

        return blobClient.Uri.AbsoluteUri;

    }


    public async Task<List<string>> UploadListBase64ImagesAsync(List<string> base64Images, string indiceContainer)
    {
        List<string> imageUrls = [];

        foreach (var base64Image in base64Images)
        {

            imageUrls.Add(await UploadBase64ImagesAsync(base64Image, indiceContainer)); // Adiciona a URL da imagem à lista
        }

        return imageUrls;
        // Retorna a lista de URLs das imagens
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

    public async Task DeleteListImagesAsync(List<string> imageNames, int indiceContainer)
    {
        foreach (var UrlImage in imageNames)
        {

            await DeleteImagesAsync(UrlImage, indiceContainer);
        }
    }

    private static string GetBlobNameFromUrl(string url)
    {
        Uri uri = new(url);
        return Path.GetFileName(uri.LocalPath);
    }
}