using DocumentFormat.OpenXml.Packaging;

namespace APIRelatorios.Infra.Relatorios.Context;

internal sealed class RelatorioContext
{
    public MainDocumentPart MainPart { get; }

    public RelatorioContext(MainDocumentPart mainPart)
    {
        MainPart = mainPart;
    }
}
