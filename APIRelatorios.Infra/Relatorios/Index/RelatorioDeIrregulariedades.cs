using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Dommain.Helpers;
using APIRelatorios.Dommain.Interfaces.Services;
using APIRelatorios.Infra.Relatorios.Context;
using APIRelatorios.Infra.Relatorios.Corpo;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace APIRelatorios.Infra.Relatorios.Index;

public class RelatorioDeIrregulariedades : IRelatorioDeIrregularidades
{
    public async Task<byte[]> BuildAsync(IEnumerable<DadosRelatorioDTO> dto)
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
                .GroupBy(d => d.Tema)
                .ToDictionary(
                    g => g.Key.ToDescricao(),
                    g => g.AsEnumerable()
                );

            var bodyRelatorio = BodyRelatorio.Criar(ctx, dadosAgrupados);
            var bodyFinal = mainPart.Document.Body;

            // ⬇️ MOVE os elementos (remove do pai antigo antes)
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