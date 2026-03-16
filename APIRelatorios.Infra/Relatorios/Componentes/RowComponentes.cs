using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Infra.Relatorios.Templates;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Wordprocessing;

namespace APIRelatorios.Infra.Relatorios.Componentes;

internal class RowComponentes
{
    internal static DocumentFormat.OpenXml.Wordprocessing.Table CriarTabela(IEnumerable<DadosRelatorioDTO> relatorioDTOs)
    {
        var table = new DocumentFormat.OpenXml.Wordprocessing.Table(
            new DocumentFormat.OpenXml.Wordprocessing.TableProperties(
                    new TableWidth
                    {
                        Type = TableWidthUnitValues.Dxa,
                        Width = "9170"
                    },
                    new TableLayout
                    {
                        Type = TableLayoutValues.Fixed
                    },
                    new DocumentFormat.OpenXml.Wordprocessing.TableBorders(
                    new DocumentFormat.OpenXml.Wordprocessing.TopBorder { Val = BorderValues.Single, Size = 4 },
                    new DocumentFormat.OpenXml.Wordprocessing.BottomBorder { Val = BorderValues.Single, Size = 4 },
                    new DocumentFormat.OpenXml.Wordprocessing.LeftBorder { Val = BorderValues.Single, Size = 4 },
                    new DocumentFormat.OpenXml.Wordprocessing.RightBorder { Val = BorderValues.Single, Size = 4 },
                    new DocumentFormat.OpenXml.Wordprocessing.InsideHorizontalBorder { Val = BorderValues.Single, Size = 4 },
                    new DocumentFormat.OpenXml.Wordprocessing.InsideVerticalBorder { Val = BorderValues.Single, Size = 4 }
                )   
            )
        );
        table.AppendChild(
        new DocumentFormat.OpenXml.Wordprocessing.TableGrid(
        new DocumentFormat.OpenXml.Wordprocessing.GridColumn { Width = "1130" }, // Item
        new DocumentFormat.OpenXml.Wordprocessing.GridColumn { Width = "5670" }, // Descrição
        new DocumentFormat.OpenXml.Wordprocessing.GridColumn { Width = "1930" }  // Fotos
    )
);

        table.Append(
        new DocumentFormat.OpenXml.Wordprocessing.TableRow(

            TitleRowComponents.TextoTitulo("Item"),
            TitleRowComponents.TextoTitulo("Descrição da Irregularidade"),
            TitleRowComponents.TextoTitulo("Fotos Ilustrativas")
        )
    );

        int indice = 0;
        foreach (var item in relatorioDTOs)
        {
            indice++;

            table.Append(
                new DocumentFormat.OpenXml.Wordprocessing.TableRow(
                    CellComponentes.Texto(indice.ToString(), "", negritoTexto1: true),
                    CellComponentes.Texto(item.Alimentador ,$" - {item.Dsc}, {item.Identificação}, {item.Localização} ", negritoTexto1: true),
                    CellComponentes.Texto(item.NumeroImagem,"", negritoTexto1: true)
                )
            );
        }

        return table;
    }
}
