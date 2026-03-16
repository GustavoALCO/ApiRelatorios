using DocumentFormat.OpenXml.Wordprocessing;

namespace APIRelatorios.Infra.Relatorios.Templates;

internal class CellComponentes
{
    internal static TableCell Texto(
        string texto,
        string? texto2,
        JustificationValues alinhamento = JustificationValues.Center,
        bool negritoTexto1 = false,
        bool negritoTexto2 = false)
    {
        var paragraph = new Paragraph(
            new ParagraphProperties(
                new Justification { Val = alinhamento }
            )
        );

        // Primeiro texto
        paragraph.Append(
            new Run(
                new RunProperties(
                    negritoTexto1 ? new Bold() : null,
                    new FontSize { Val = "22" }
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
                        new FontSize { Val = "22"}
                    ),
                    new Text(texto2)
                )
            );
        }

        return new TableCell(
            new TableCellProperties(
                new TableCellVerticalAlignment
                {
                    Val = TableVerticalAlignmentValues.Center
                },
                new Shading
                {
                    Val = ShadingPatternValues.Clear,
                    Color = "auto",
                    Fill = "F8F8FF"
                }
            ),
            paragraph
        );
    }
}