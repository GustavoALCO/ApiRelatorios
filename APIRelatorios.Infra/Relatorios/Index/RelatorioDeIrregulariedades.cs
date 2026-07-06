using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Dommain.Helpers;
using APIRelatorios.Dommain.Interfaces.Services;
using APIRelatorios.Infra.Relatorios.Context;
using APIRelatorios.Infra.Relatorios.Corpo;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Infra.Relatorios.Index;

public class RelatorioDeIrregulariedades : IRelatorioDeIrregularidades
{
    private readonly ILogger<RelatorioDeIrregulariedades> _logger;

    public RelatorioDeIrregulariedades(
        ILogger<RelatorioDeIrregulariedades> logger)
    {
        _logger = logger;
    }

    public async Task<byte[]> BuildAsync(
        IEnumerable<DadosRelatorioDTO> dto)
    {
        using var ms = new MemoryStream();

        using (var doc = WordprocessingDocument.Create(
            ms,
            WordprocessingDocumentType.Document,
            true))
        {
            var mainPart = doc.AddMainDocumentPart();

            mainPart.Document = new Document(new Body());

            var ctx = new RelatorioContext(mainPart);

            var dadosAgrupados = dto
            .Select(d =>
            {
                if (string.IsNullOrWhiteSpace(d.Tema))
                {
                    _logger.LogWarning(
                        """
                        Evidência sem Tema.
                        NumeroImagem: {NumeroImagem}
                        Alimentador: {Alimentador}
                        Observacao: {Observacao}
                        Irregularidades: {Irregularidades}
                        """,
                        d.NumeroImagem,
                        d.Alimentador,
                        d.Observacao,
                        d.Irregularidades
                    );
                }

                if (string.IsNullOrWhiteSpace(d.Irregularidades))
                {
                    _logger.LogWarning(
                        """
                        Evidência com TemaCheck porém sem irregularidades.
                        TemaCheck: {TemaCheck}
                        NumeroImagem: {NumeroImagem}
                        Alimentador: {Alimentador}
                        Observacao: {Observacao}
                        Irregularidades: {Irregularidades}
                        """,
                        d.Tema,
                        d.NumeroImagem,
                        d.Alimentador,
                        d.Observacao,
                        d.Irregularidades
                    );
                }

                return new
                {
                    TemaCheck = string.IsNullOrWhiteSpace(d.Tema)
                        ? "Sem classificação"
                        : d.Tema,

                    Item = d
                };
            })
            .GroupBy(x => x.TemaCheck)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Item).ToList()
            );

            _logger.LogInformation(
                "Temas encontrados no relatório: {Temas}",
                string.Join(", ", dadosAgrupados.Keys)
            );

            var bodyRelatorio =
                BodyRelatorio.Criar(ctx, dadosAgrupados);

            var bodyFinal = mainPart.Document.Body;

            foreach (var element in bodyRelatorio.Elements().ToList())
            {
                element.Remove();

                bodyFinal.Append(element);
            }

            mainPart.Document.Save();
        }

        return ms.ToArray();
    }
}