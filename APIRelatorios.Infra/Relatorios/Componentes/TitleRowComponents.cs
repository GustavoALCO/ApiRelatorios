using DocumentFormat.OpenXml.Wordprocessing;

namespace APIRelatorios.Infra.Relatorios.Componentes;

internal class TitleRowComponents
{
    internal static TableCell TextoTitulo(string texto)
    {
        return new TableCell(
            new TableCellProperties(
                
                new Shading
                {
                    Val = ShadingPatternValues.Clear,
                    Fill = "D9D9D9"
                }
            ),
            new Paragraph(
                new ParagraphProperties(
                    new Justification { Val = JustificationValues.Center }
                ),
                new Run(
                    new RunProperties(new Bold()),
                    new Text(texto)
                )
            )
        );
    }
}
