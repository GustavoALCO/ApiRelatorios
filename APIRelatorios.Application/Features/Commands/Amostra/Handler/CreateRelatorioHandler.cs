using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Exceptions.NotFound;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Domain.Interfaces.Amostra;
using APIRelatorios.Dommain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.Amostra.Handler;

public class CreateRelatorioHandler : ICommandHandler<CreateRelatorioAmostraCommand, byte[]>
{

    private readonly IAmostraQuery _amostraQuery;

    private readonly ILogger<CreateRelatorioHandler> _logger;

    private readonly IBuscarByteImagemService _buscarByteImageService;

    private readonly IRelatorioDeIrregularidades _relatorio;

    private readonly IZipService _zipService;

    public CreateRelatorioHandler(IAmostraQuery amostraQuery, ILogger<CreateRelatorioHandler> logger, IBuscarByteImagemService buscarByteImageService, IRelatorioDeIrregularidades relatorio, IZipService zipService)
    {
        _amostraQuery = amostraQuery;
        _logger = logger;
        _buscarByteImageService = buscarByteImageService;
        _relatorio = relatorio;
        _zipService = zipService;
    }

    public async Task<byte[]> Handle(CreateRelatorioAmostraCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando Processo de Gerar Tabelas da Amostra");

        var amostra = await _amostraQuery.GetAmostraCheck(command.idrota) ?? throw new AmostraNotFoundException();

        _logger.LogInformation("Amostras que foram modificadas buscada com sucesso");

        List<(Func<Task<Stream>> StreamFactory, string Nome)> listImages = new();

        List<DadosRelatorioDTO> dto = new();

        foreach(var am in amostra)
        {
            int index = 0;
            
            foreach(var image in am.Fotos)
            {
                index++;

                var byteImage = await _buscarByteImageService.BaixarImagemAsync(image);


                //montando campo de observacao
                var irregularidades = new List<string>();

                if (am.TUC1 != null)
                    irregularidades.Add($"TUC1: {am.TUC1}");

                if (am.TUC2 != null)
                    irregularidades.Add($"TUC2: {am.TUC2}");

                if (am.TUC3 != null)
                    irregularidades.Add($"TUC3: {am.TUC3}");

                if (am.TUC4 != null)
                    irregularidades.Add($"TUC4: {am.TUC4}");

                if (am.TUC5 != null)
                    irregularidades.Add($"TUC4: {am.TUC5}");

                if (am.TUC6 != null)
                    irregularidades.Add($"TUC5: {am.TUC6}");

                if(am.NumSerie != null)
                    irregularidades.Add($"NumSerie: {am.NumSerie}");

                if(am.PosicaoOperativa !=  null)
                    irregularidades.Add($"Posição Operativa: {am.NumSerie}");

                if(am.Equipamento != null)
                    irregularidades.Add($"Equipamento: {am.Equipamento}");

                if (am.DataFabricacao != null)
                    irregularidades.Add($"DataFabricacao: {am.DataFabricacao}");

                if (am.Observacao != null)
                    irregularidades.Add($"Observacao: {am.Observacao}");

                var texto = string.Join(", ", irregularidades);
                DadosRelatorioDTO dadosImplantados = new()
                {
                    Foto = byteImage,

                    Alimentador = am.SeqISA,

                    Tema = am.SeqBaseFisica,

                    NumeroImagem = index.ToString(),

                    Identificação = am.DescricaoTec,

                    Localização = $"{am.Endereco} - {am.Municipio}",

                    Observacao = $"{am.DescricaoTec} - {texto}",

                };

                dto.Add( dadosImplantados );

                var originalUrl = image;

                listImages.Add((
                    async () =>
                    {
                        var bytes = await _buscarByteImageService.BaixarImagemAsync(originalUrl);
                        return new MemoryStream(bytes);
                    },
                    $"{am.SeqISA}-{index}.jpg"
                ));

            }
            
        }

        var bytesRelatorio = await _relatorio.BuildAsync(dto);

        var bytesZip = await _zipService.CreateZipWithImagesAsync(
    bytesRelatorio,
    listImages);
        throw new NotImplementedException();
    }
}