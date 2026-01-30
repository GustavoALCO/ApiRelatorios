using DocumentFormat.OpenXml.Wordprocessing;

namespace APIRelatorios.Infra.Relatorios.Templates;

internal class CellComponentes
{
    internal static TableCell Texto(
        string texto,
        JustificationValues alinhamento = JustificationValues.Center,
        bool negrito = false)
    {
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
            new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = alinhamento }
                ),
                new Run(
                    new RunProperties(
                        negrito ? new Bold() : null
                    ),
                    new Text(texto)
                )
            )
        );
    }
}
