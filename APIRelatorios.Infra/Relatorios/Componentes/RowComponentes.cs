using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Infra.Relatorios.Templates;
using DocumentFormat.OpenXml.Wordprocessing;

namespace APIRelatorios.Infra.Relatorios.Componentes;

internal class RowComponentes
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
                new GridColumn { Width = "1130" },
                new GridColumn { Width = "5670" },
                new GridColumn { Width = "1930" }
            )
        );

        table.Append(
            new TableRow(
                TitleRowComponents.TextoTitulo("Item"),
                TitleRowComponents.TextoTitulo("Descrição da Irregularidade"),
                TitleRowComponents.TextoTitulo("Fotos Ilustrativas")
            )
        );

        int indice = 0;

        var tabelaAgrupada = relatorioDTOs
            .GroupBy(x => x.NumeroImagem)
            .Select(x => x.First())
            .ToList();

        foreach (var item in tabelaAgrupada)
        {
            indice++;

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

            table.Append(
                new TableRow(
                    CellComponentes.Texto(
                        indice.ToString(),
                        "",
                        negritoTexto1: true
                    ),

                    CellComponentes.Texto(
                        item.Alimentador ?? "",
                        descricao,
                        negritoTexto1: true
                    ),

                    CellComponentes.Texto(
                        item.NumeroImagem ?? "",
                        "",
                        negritoTexto1: true
                    )
                )
            );
        }

        return table;
    }
}