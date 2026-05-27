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
                .SelectMany(d =>
                {
                    // =========================
                    // LOGS DE VALIDAÇÃO
                    // =========================

                    if (d.Tema == null)
                    {
                        _logger.LogWarning(
                            """
                            Evidência sem Tema.
                            NumeroImagem: {NumeroImagem}
                            Alimentador: {Alimentador}
                            Dsc: {Descricao}
                            """,
                            d.NumeroImagem,
                            d.Alimentador,
                            d.Dsc
                        );

                        return new[]
                        {
                            new
                            {
                                SubTema = "Sem classificação",
                                Item = d
                            }
                        };
                    }

                    if (d.Tema.SubTemaAlimentadores == null)
                    {
                        _logger.LogWarning(
                            """
                            Evidência com Tema porém sem SubTemaAlimentadores.
                            NumeroImagem: {NumeroImagem}
                            Alimentador: {Alimentador}
                            Dsc: {Descricao}
                            """,
                            d.NumeroImagem,
                            d.Alimentador,
                            d.Dsc
                        );

                        return new[]
                        {
                            new
                            {
                                SubTema = "Sem classificação",
                                Item = d
                            }
                        };
                    }

                    if (!d.Tema.SubTemaAlimentadores.Any())
                    {
                        _logger.LogWarning(
                            """
                            Evidência com lista SubTemaAlimentadores vazia.
                            NumeroImagem: {NumeroImagem}
                            Alimentador: {Alimentador}
                            Dsc: {Descricao}
                            """,
                            d.NumeroImagem,
                            d.Alimentador,
                            d.Dsc
                        );

                        return new[]
                        {
                            new
                            {
                                SubTema = "Sem classificação",
                                Item = d
                            }
                        };
                    }

                    // =========================
                    // NORMAL
                    // =========================

                    return d.Tema.SubTemaAlimentadores.Select(st => new
                    {
                        SubTema = st.ToDescricao(),
                        Item = d
                    });
                })
                .GroupBy(x => x.SubTema)
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