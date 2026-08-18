using DocumentFormat.OpenXml.Wordprocessing;

namespace APIRelatorios.Infra.Relatorios.Templates;

internal class CellComponentes
{
    internal static TableCell Texto(
        string texto,
        string? texto2,
        JustificationValues? alinhamento = null,
        bool negritoTexto1 = false,
        bool negritoTexto2 = false)
    {
        var paragraphProperties = new ParagraphProperties();

        if (alinhamento.HasValue)
        {
            paragraphProperties.Append(
                new Justification { Val = alinhamento.Value }
            );
        }

        var paragraph = new Paragraph(paragraphProperties);

        // Primeiro texto
        paragraph.Append(
            new Run(
                new RunProperties(
                    negritoTexto1 ? new Bold() : null,
                    new FontSize { Val = "18" }
                ),
                new Text(texto)
            )
        );

        // Segundo texto (se existir)
        if (!string.IsNullOrEmpty(texto2))
        {
            paragraph.Append(new Run());

            paragraph.Append(
                new Run(
                    new RunProperties(
                        negritoTexto2 ? new Bold() : null,
                        new FontSize { Val = "18"}
                    ),
                    new Text(texto2)
                )
            );
        }

        return new TableCell(
            new TableCellProperties(
                new Shading
                {
                    Val = ShadingPatternValues.Clear,
                    Color = "auto",
                    Fill = "F8F8FF"
                },
                new TableCellVerticalAlignment
                {
                    Val = TableVerticalAlignmentValues.Center
                }
            ),
            paragraph
        );
    }
}
