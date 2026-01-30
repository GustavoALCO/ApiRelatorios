using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Dommain.Helpers;
using APIRelatorios.Dommain.Interfaces.Services;
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
            mainPart.Document = new Document();
            var body = mainPart.Document.AppendChild(new Body());

            // Agrupa os dados
            var dadosAgrupados = dto
                .GroupBy(d => d.Tema)
                .ToDictionary(
                    grupo => grupo.Key.ToDescricao(),
                    grupo => grupo.AsEnumerable()
                );

            // Cria o body do relatório
            var bodyRelatorio = BodyRelatorio.Criar(dadosAgrupados);

            // Adiciona todos os elementos ao body real
            foreach (var element in bodyRelatorio.Elements())
            {
                body.Append(element.CloneNode(true));
            }

            mainPart.Document.Save();
        }

        return ms.ToArray();
    }
}