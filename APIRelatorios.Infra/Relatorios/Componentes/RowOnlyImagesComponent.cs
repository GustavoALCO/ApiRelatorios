using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Infra.Relatorios.Context;
using APIRelatorios.Infra.Relatorios.Templates;
using DocumentFormat.OpenXml.Wordprocessing;

namespace APIRelatorios.Infra.Relatorios.Componentes;

internal class RowOnlyImagesComponent
{
    private const int MaxColunas = 2;

    internal static Table CriarTabelasImagem(
        RelatorioContext ctx,
        IEnumerable<DadosRelatorioDTO> relatorioDTOs)
    {
        var table = CriarTabelaBase();

        var grupos = relatorioDTOs.Chunk(MaxColunas);

        foreach (var grupo in grupos)
        {
            var rowImages = new TableRow();

            foreach (var item in grupo)
            {
                rowImages.Append(
                    ImageCellComponentes.Criar(
                        ctx,
                        item.Foto
                    )
                );
            }

            PreencherCelulasVazias(rowImages);

            table.Append(rowImages);
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
                    new TopBorder
                    {
                        Val = BorderValues.Single,
                        Size = 4
                    },

                    new BottomBorder
                    {
                        Val = BorderValues.Single,
                        Size = 4
                    },

                    new LeftBorder
                    {
                        Val = BorderValues.Single,
                        Size = 4
                    },

                    new RightBorder
                    {
                        Val = BorderValues.Single,
                        Size = 4
                    },

                    new InsideHorizontalBorder
                    {
                        Val = BorderValues.Single,
                        Size = 4
                    },

                    new InsideVerticalBorder
                    {
                        Val = BorderValues.Single,
                        Size = 4
                    }
                )
            )
        );

        table.AppendChild(
            new TableGrid(
                new GridColumn { Width = "4585" },
                new GridColumn { Width = "4585" }
            )
        );

        return table;
    }

    private static void PreencherCelulasVazias(TableRow row)
    {
        while (row.ChildElements.Count < MaxColunas)
        {
            row.Append(
                CellComponentes.Texto("", "")
            );
        }
    }
}