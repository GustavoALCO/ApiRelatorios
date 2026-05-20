using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Application.Contracts.Enum;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Dommain.Helpers;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.Rota.Handler;

public class CreateRelatorioHandler
{
    private readonly IRelatorioDeIrregularidades _relatorio;
    private readonly IEvidenciaRotaQuery _rotaQuery;
    private readonly IImagesQuery _imageQuery;
    private readonly IBuscarByteImagemService _byteImage;
    private readonly IValidateIds _validateIds;
    private readonly IZipService _zipService;
    private readonly ILogger<CreateRelatorioHandler> _logger;

    public CreateRelatorioHandler(
        IEvidenciaRotaQuery rotaQuery,
        IRelatorioDeIrregularidades relatorio,
        IBuscarByteImagemService byteImage,
        IValidateIds validateIds,
        IRotaQuery rotaquery,
        IImagesQuery imageQuery,
        ILogger<CreateRelatorioHandler> logger,
        IZipService zipService)
    {
        _rotaQuery = rotaQuery;
        _relatorio = relatorio;
        _byteImage = byteImage;
        _validateIds = validateIds;
        _imageQuery = imageQuery;
        _logger = logger;
        _zipService = zipService;
    }

    public async Task<byte[]> Handler(CreateRelatorioWordCommand command)
    {
        try
        {
            ICollection<DadosRelatorioDTO> evidencias = new List<DadosRelatorioDTO>();

            List<(Func<Task<Stream>> StreamFactory, string Nome)> fotos =
                new();

            _logger.LogInformation(
                "Verificando se a lista de rotas existem no banco de dados");

            foreach (var rota in command.Ids)
            {
                var existe = await _validateIds.RotaExisteAsync(rota);

                if (!existe)
                {
                    throw new Exception(
                        $"A rota {rota} não existe.");
                }
            }

            for (int i = 0; i < command.Ids.Count; i++)
            {
                _logger.LogInformation(
                    "Buscando rota do index {Index}",
                    i);

                var evidenciasBruto =
                    await _rotaQuery.GetEvidenciaAsync(command.Ids[i]);

                int contagem = 1;

                foreach (var evidencia in evidenciasBruto)
                {
                    _logger.LogInformation(
                        "Processando evidência {Contagem} da rota {Index}",
                        contagem,
                        i);

                    var images =
                        await _imageQuery.GetImageEvidencia(
                            evidencia.EvidenciaRotaId);

                    int contadorImagem = 1;

                    foreach (var image in images)
                    {
                        try
                        {
                            var nomeImagem =
                                $"{(EnumLetras)i} - {contagem}.{contadorImagem}";

                            _logger.LogInformation(
                                "Baixando imagem: {NomeImagem}",
                                nomeImagem);

                            var lowBytes =
                                await _byteImage.BaixarImagemAsync(
                                    image.LowUrl);

                            _logger.LogInformation(
                                "Imagem {NomeImagem} baixada com sucesso. Bytes: {Bytes}",
                                nomeImagem,
                                lowBytes.Length);

                            var descricaoSubTemas =
                                string.Join(
                                    ", ",
                                    evidencia.CheckList?
                                        .SubTemaAlimentadores?
                                        .Select(x => x.ToDescricao())
                                    ?? Enumerable.Empty<string>());

                            DadosRelatorioDTO dto = new()
                            {
                                Foto = lowBytes,

                                Dsc =
                                    $"{descricaoSubTemas}, " +
                                    $"{evidencia.Descricao ?? "Observação vazia"}",

                                Alimentador =
                                    evidencia.Alimentador ?? "",

                                Identificação =
                                    evidencia.Identificacão ?? "",

                                Localização =
                                    evidencia.Endereco ?? "",

                                NumeroImagem =
                                    nomeImagem,

                                Tema =
                                    evidencia.CheckList,
                            };

                            evidencias.Add(dto);

                            var originalUrl = image.OriginalUrl;

                            fotos.Add((
                                async () =>
                                {
                                    var bytes =
                                        await _byteImage.BaixarImagemAsync(
                                            originalUrl);

                                    return new MemoryStream(bytes);
                                },

                                $"{nomeImagem}.jpg"
                            ));

                            _logger.LogInformation(
                                "Imagem {NomeImagem} adicionada com sucesso",
                                nomeImagem);

                            contadorImagem++;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(
                                ex,
                                "Erro ao processar imagem da evidência {EvidenciaId}",
                                evidencia.EvidenciaRotaId);

                            throw;
                        }
                    }

                    contagem++;
                }
            }

            _logger.LogInformation(
                "Iniciando geração do relatório Word");

            var bytesRelatorio =
                await _relatorio.BuildAsync(evidencias);

            _logger.LogInformation(
                "Relatório Word gerado com sucesso");

            _logger.LogInformation(
                "Iniciando criação do ZIP");

            var bytesZip =
                await _zipService.CreateZipWithImagesAsync(
                    bytesRelatorio,
                    fotos);

            _logger.LogInformation(
                "ZIP criado com sucesso");

            return bytesZip;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro geral ao gerar relatório");

            throw;
        }
    }
}