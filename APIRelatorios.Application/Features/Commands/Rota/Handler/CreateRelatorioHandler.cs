using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Contracts.Enum;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class CreateRelatorioHandler
{
    private readonly IRelatorioDeIrregularidades _relatorio;

    private readonly IEvidenciaRotaQuery _RotaQuery;

    private readonly IImagesQuery _imageQuery;

    private readonly IBuscarByteImagem _ByteImage;

    private readonly IValidateIds _validateIds;

    private readonly ILogger<CreateRelatorioHandler> _logger;

    public CreateRelatorioHandler(IEvidenciaRotaQuery rotaQuery, IRelatorioDeIrregularidades relatorio, IBuscarByteImagem byteImage, IValidateIds validateIds, IRotaQuery rotaquery, IImagesQuery imageQuery, ILogger<CreateRelatorioHandler> logger)
    {
        _RotaQuery = rotaQuery;
        _relatorio = relatorio;
        _ByteImage = byteImage;
        _validateIds = validateIds;
        _imageQuery = imageQuery;
        _logger = logger;
    }

    public async Task<byte[]> Handler(CreateRelatorioWordCommand command)
    {
        ICollection<DadosRelatorioDTO> evidencias = [];

        _logger.LogInformation("Verificando se a lista de rotas existem no banco de dados");

        foreach (var rota in command.Ids)
        {
            if (await _validateIds.RotaExisteAsync(rota) is false)
                throw new Exception("Erro na lista de ids");
        }

        for (int i = 0; i < command.Ids.Count; i++)
        {
            _logger.LogInformation($"Buscando rota do index : {i}");
            var evidenciasBruto = await _RotaQuery.GetEvidenciaAsync(command.Ids[i]);

            foreach (var evidenciasloop in evidenciasBruto)
            {
                var images = await _imageQuery.GetImageEvidencia(evidenciasloop.EvidenciaRotaId);

                int contagem = 1;
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
                        Localização = $"{evidenciasloop.Endereco}",
                        NumeroImagem = $"{(EnumLetras)i} - {contagem}",
                        Tema = evidenciasloop.TemaFiscalizacao,
                    };

                    evidencias.Add(evidendcias);

                    _logger.LogInformation($"Evidencia do index {i}");
                }

                contagem++;
            }
        }

        var bytesRelatorio = await _relatorio.BuildAsync(evidencias);

        return bytesRelatorio;
        
    }
}
