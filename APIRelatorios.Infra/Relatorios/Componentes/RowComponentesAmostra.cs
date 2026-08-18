using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Infra.Relatorios.Templates;
using DocumentFormat.OpenXml.Drawing.Wordprocessing;
using DocumentFormat.OpenXml.Wordprocessing;

namespace APIRelatorios.Infra.Relatorios.Componentes;

internal class RowComponentesAmostra
{
    internal static Table CriarTabela(
         IEnumerable<DadosRelatorioDTO> relatorioDTOs)
    {
        var table = new Table(
            new TableProperties(
                new TableWidth
                {
                    Type = TableWidthUnitValues.Dxa,
                    Width = "9170"
                },

                new TableBorders(
                    new TopBorder
                    {
                        Val = BorderValues.Single,
                        Size = 4
                    },

                    new LeftBorder
                    {
                        Val = BorderValues.Single,
                        Size = 4
                    },

                    new BottomBorder
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
                ),

                new TableLayout
                {
                    Type = TableLayoutValues.Fixed
                }
            )
        );

        table.AppendChild(
            new TableGrid(
                new GridColumn { Width = "9170" }
            )
        );

        table.Append(
            new TableRow(
                TitleRowComponents.TextoTitulo("Descrição")
            )
        );

        foreach (var item in relatorioDTOs)
        {
            var titulo = string.Join(
                " - ",
                new[]
                {
                    item.Alimentador,
                    item.Irregularidades
                }
                .Where(x => !string.IsNullOrWhiteSpace(x))
            );

            var descricao = string.Join(
                ", ",
                new[]
                {
                    item.Observacao,
                    item.Identificação,
                    item.Localização
                }
                .Where(x => !string.IsNullOrWhiteSpace(x))
            );

            if (!string.IsNullOrWhiteSpace(descricao))
            {
                titulo += ",";
            }

            table.Append(
                new TableRow(
                    CellComponentes.Texto(
                        titulo,
                        descricao,
                        negritoTexto1: true
                    )
                )
            );
        }

        return table;
    }
}
