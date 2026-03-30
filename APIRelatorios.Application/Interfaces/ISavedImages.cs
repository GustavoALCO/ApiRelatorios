using APIRelatorios.Dommain.Entities;

namespace ChatApplication.Application.Interfaces;

public interface ISavedImages
{
    public Task<string> UploadBase64ImagesAsync(string alimentador,
        string fiscal,
        string horario,
        string pluscode,
        string base64Image,
        string container,
        string qualidade,
        int index);

    public Task<List<ImageData>> UploadListBase64ImagesAsync(
    string alimentador,
        string fiscal,
        string horario,
        List<string> base64Images,
        string container,
        Guid evidenciaId,
        double lat,
        double log
);

    public byte[] RedimensionarImagem(byte[] imageBytes, int largura, int qualidade);

}