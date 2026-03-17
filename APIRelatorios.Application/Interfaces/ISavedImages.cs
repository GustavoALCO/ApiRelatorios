namespace ChatApplication.Application.Interfaces;

public interface ISavedImages
{
    public Task<string> UploadBase64ImagesAsync(string alimentador,
                                                string fiscal,
                                                string horario,
                                                string base64Image,
                                                string container,
                                                int index);

    Task<List<string>> UploadListBase64ImagesAsync
                                    (string alimentador,
                                    string fiscal,
                                    string horario,
                                    List<string> base64Image,
                                    string container
                                    );

    public Task DeleteImagesAsync(string imageNames,
                                  int indiceContainer);

}