using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Infra.Relatorios.Templates;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace APIRelatorios.Infra.Relatorios.Componentes;

internal class RowImagesComponentes
{
    internal static Table CriarTabelasImagem(
    MainDocumentPart mainPart,
    IEnumerable<DadosRelatorioDTO> relatorioDTOs)
    {
        var table = CriarTabelaBase();

        var grupos = relatorioDTOs.Chunk(3);

        foreach (var grupo in grupos)
        {
            // Linha de imagens
            var rowImages = new TableRow();
            foreach (var item in grupo)
            {
                rowImages.Append(
                    ImageCellComponentes.Criar(
                        mainPart,
                        item.Foto,
                        2400000,
                        1800000
                    )
                );
            }
            PreencherCelulasVazias(rowImages);
            table.Append(rowImages);

            // Linha de descrições
            var rowDesc = new TableRow();
            foreach (var item in grupo)
            {
                rowDesc.Append(
                    CellComponentes.Texto(item.Dsc)
                );
            }
            PreencherCelulasVazias(rowDesc);
            table.Append(rowDesc);
        }

        return table;
    }


    private static Table CriarTabelaBase()
    {
        var table = new Table(
            new TableProperties(
                new TableWidth
                {
                    Type = TableWidthUnitValues.Dxa,
                    Width = "9170"
                },
                new TableLayout
                {
                    Type = TableLayoutValues.Fixed
                },
                new TableBorders(
                    new TopBorder { Val = BorderValues.Single, Size = 4 },
                    new BottomBorder { Val = BorderValues.Single, Size = 4 },
                    new LeftBorder { Val = BorderValues.Single, Size = 4 },
                    new RightBorder { Val = BorderValues.Single, Size = 4 },
                    new InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                    new InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                )
            )
        );

        table.AppendChild(
            new TableGrid(
                new GridColumn { Width = "3050" },
                new GridColumn { Width = "3050" },
                new GridColumn { Width = "3050" }
            )
        );

        return table;
    }

    private static void PreencherCelulasVazias(TableRow row)
    {
        while (row.ChildElements.Count < 3)
        {
            row.Append(CellComponentes.Texto(""));
        }
    }
}
