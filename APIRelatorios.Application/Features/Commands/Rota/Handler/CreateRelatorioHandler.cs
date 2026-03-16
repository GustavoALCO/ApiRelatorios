using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Contracts.Enum;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.Services;
using Google.OpenLocationCode;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class CreateRelatorioHandler
{
    private readonly IRelatorioDeIrregularidades _relatorio;

    private readonly IEvidenciaRotaQuery _RotaQuery;

    private readonly IBuscarByteImagem _ByteImage;

    private readonly IValidateIds _validateIds;

    private readonly IRotaQuery _rotaQuery;

    public CreateRelatorioHandler(IEvidenciaRotaQuery rotaQuery, IRelatorioDeIrregularidades relatorio, IBuscarByteImagem byteImage, IValidateIds validateIds, IRotaQuery rotaquery)
    {
        _RotaQuery = rotaQuery;
        _relatorio = relatorio;
        _ByteImage = byteImage;
        _validateIds = validateIds;
        _rotaQuery = rotaquery;
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
            var evidenciasBruto = await _RotaQuery.GetImagemAsync(command.Ids[i]);

            int contagem = 1;
            foreach (var evidendciasloop in evidenciasBruto)
            {
                string plusCode = OpenLocationCode.Encode(evidendciasloop.Latitude, evidendciasloop.Longitude);

                DadosRelatorioDTO evidendcias = new()
                {
                    // Buscar os bytes da imagem 
                    Foto = await _ByteImage.BaixarImagemAsync(evidendciasloop.ImageURL),
                    Dsc = 
                    $"{evidendciasloop.Descricao ?? "DSC VAZIO"} ",
                    Alimentador = $"{evidendciasloop.Alimentador ??  "Aliemntador Não Declarado"}",
                    Identificação = $"{evidendciasloop.Identificacão}",
                    Localização = $"{evidendciasloop.Endereco ?? "ENDEREÇO VAZIO"} - PlusCode : {plusCode}",
                    NumeroImagem = $"{(EnumLetras)i} - {contagem}" ,
                    Tema = evidendciasloop.TemaFiscalizacao ,
                };

                contagem++;

                evidencias.Add(evidendcias);
            }
        }
        

        var bytesRelatorio = await _relatorio.BuildAsync(evidencias);

        return bytesRelatorio;
        
    }
}
