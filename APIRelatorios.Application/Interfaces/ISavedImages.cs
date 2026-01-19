namespace ChatApplication.Application.Interfaces;

public interface ISavedImages
{
    public Task<string> UploadBase64ImagesAsync(string base64Images, string container);

    public Task<List<string>> UploadListBase64ImagesAsync(List<string> base64Images, string indiceContainer);

    public Task DeleteImagesAsync(string imageNames, int indiceContainer);

    public Task DeleteListImagesAsync(List<string> imageNames, int indiceContainer);


}