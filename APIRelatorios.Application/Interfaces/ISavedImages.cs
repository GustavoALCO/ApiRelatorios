namespace ChatApplication.Application.Interfaces;

public interface ISavedImages
{
    public Task<string> UploadBase64ImagesAsync(string alimentador,string fiscal, string horario, string base64Image, string container);

    public Task DeleteImagesAsync(string imageNames, int indiceContainer);

}