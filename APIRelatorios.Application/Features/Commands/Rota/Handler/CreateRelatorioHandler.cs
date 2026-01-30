using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Contracts.Enum;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.Services;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class CreateRelatorioHandler
{
    private readonly IRelatorioDeIrregularidades _relatorio;

    private readonly IEvidenciaRotaQuery _RotaQuery;

    private readonly IBuscarByteImagem _ByteImage;

    public CreateRelatorioHandler(IEvidenciaRotaQuery rotaQuery, IRelatorioDeIrregularidades relatorio, IBuscarByteImagem byteImage)
    {
        _RotaQuery = rotaQuery;
        _relatorio = relatorio;
        _ByteImage = byteImage;
    }

    public async Task<byte[]> Handler(CreateRelatorioWordCommand command)
    {
        ICollection<DadosRelatorioDTO> evidencias = [];

        for (int i = 0; i < command.Ids.Length; i++)
        {
            var evidenciasBruto = await _RotaQuery.GetImagemAsync(command.Ids[i]);

            int contagem = 1;
            foreach (var evidendciasloop in evidenciasBruto)
            {
                DadosRelatorioDTO evidendcias = new()
                {
                    // Buscar os bytes da imagem 
                    Foto = await _ByteImage.BaixarImagemAsync(evidendciasloop.ImageURL),
                    Dsc =
                    $"{evidendciasloop.Alimentador ?? "ALIMENTADOR VAZIO"} " +
                    $"{evidendciasloop.Descricao ?? "DSC VAZIO"} " +
                    $"{evidendciasloop.Endereco ?? "ENDEREÇO VAZIO"} " +
                    $"{evidendciasloop.Cep ?? "CEP VAZIO"}",

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
