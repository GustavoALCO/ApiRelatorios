using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Contracts.Enum;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.Services;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class CreateRelatorioHandler
{
    private readonly IRelatorioDeIrregularidades _relatorio;

    private readonly IEvidenciaRotaQuery _RotaQuery;

    private readonly IImagesQuery _imageQuery;

    private readonly IBuscarByteImagem _ByteImage;

    private readonly IValidateIds _validateIds;

    public CreateRelatorioHandler(IEvidenciaRotaQuery rotaQuery, IRelatorioDeIrregularidades relatorio, IBuscarByteImagem byteImage, IValidateIds validateIds, IRotaQuery rotaquery, IImagesQuery imageQuery)
    {
        _RotaQuery = rotaQuery;
        _relatorio = relatorio;
        _ByteImage = byteImage;
        _validateIds = validateIds;
        _imageQuery = imageQuery;
    }

    public async Task<byte[]> Handler(CreateRelatorioWordCommand command)
    {
        ICollection<DadosRelatorioDTO> evidencias = [];

        foreach(var rota in command.Ids)
        {
            if (await _validateIds.RotaExisteAsync(rota) is false)
                throw new Exception("Erro na lista de ids");
        }

        for (int i = 0; i < command.Ids.Length; i++)
        {
            var evidenciasBruto = await _RotaQuery.GetEvidenciaAsync(command.Ids[i]);

            int contagem = 1;
            foreach (var evidenciasloop in evidenciasBruto)
            {
                var images = await _imageQuery.GetImageEvidencia(evidenciasloop.EvidenciaRotaId);

                foreach(var image in images)
                {
                    DadosRelatorioDTO evidendcias = new()
                    {
                        // Buscar os bytes da imagem 
                        Foto = await _ByteImage.BaixarImagemAsync(image.LowUrl),
                        Dsc =
                        $"{evidenciasloop.Descricao ?? "DSC VAZIO"} ",
                        Alimentador = $"{evidenciasloop.Alimentador ?? "Aliemntador Não Declarado"}",
                        Identificação = $"{evidenciasloop.Identificacão}",
                        Localização = $"{evidenciasloop.Endereco ?? "ENDEREÇO VAZIO"}",
                        NumeroImagem = $"{(EnumLetras)i} - {contagem}",
                        Tema = evidenciasloop.TemaFiscalizacao,
                    };

                    evidencias.Add(evidendcias);
                }
                
                contagem++;
                
            }
        }
        

        var bytesRelatorio = await _relatorio.BuildAsync(evidencias);

        return bytesRelatorio;
        
    }
}
