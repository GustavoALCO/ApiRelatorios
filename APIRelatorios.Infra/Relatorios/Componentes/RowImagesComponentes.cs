using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Infra.Relatorios.Context;
using APIRelatorios.Infra.Relatorios.Templates;
using DocumentFormat.OpenXml.Wordprocessing;

namespace APIRelatorios.Infra.Relatorios.Componentes;

internal class RowImagesComponentes
{
    internal static Table CriarTabelasImagem(
        RelatorioContext ctx,
        IEnumerable<DadosRelatorioDTO> relatorioDTOs)
    {
        var table = CriarTabelaBase();

        // Quebra em grupos de 3
        var grupos = relatorioDTOs.Chunk(3);

        foreach (var grupo in grupos)
        {
            // =========================
            // LINHA DAS IMAGENS
            // =========================
            var rowImages = new TableRow();

            foreach (var item in grupo)
            {
                rowImages.Append(
                    ImageCellComponentes.Criar(
                        ctx,
                        item.Foto,
                        1800000,
                        1800000
                    )
                );
            }

            PreencherCelulasVazias(rowImages);

            table.Append(rowImages);

            // =========================
            // LINHA DAS DESCRIÇÕES
            // =========================
            var rowDesc = new TableRow();

            foreach (var item in grupo)
            {
                var titulo = string.Join(
                    " - ",
                    new[]
                    {
                        item.NumeroImagem,
                        item.Alimentador
                    }
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                );

                var descricao = string.Join(
                    ", ",
                    new[]
                    {
                        item.Dsc,
                        item.Identificação,
                        item.Localização
                    }
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                );

                rowDesc.Append(
                    CellComponentes.Texto(
                        titulo,
                        descricao,
                        negritoTexto1: true
                    )
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

        // Colunas fixas
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
            row.Append(
                CellComponentes.Texto("", "")
            );
        }
    }
}