using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Infra.Relatorios.Componentes;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace APIRelatorios.Infra.Relatorios.Corpo;

internal class BodyRelatorio
{
    internal static Body Criar(
                
                IDictionary<string, IEnumerable<DadosRelatorioDTO>> dadosPorTema
        )
    {
        var body = new Body();

        foreach (var tema in dadosPorTema)
        {
            // Título do tema
            body.Append(CriarTituloTema(tema.Key));

            // Tabela do tema
            body.Append(
                RowComponentes.CriarTabela(tema.Value)
            );

        }

        //foreach (var tema in dadosPorTema)
        //{
        //    // Título do tema
        //    body.Append(CriarTituloTema(tema.Key));

        //    // Tabela do tema
        //    body.Append(
        //        RowImagesComponentes.CriarTabelasImagem(
        //            mainPart,
        //            tema.Value)
        //    );

        //}
        

        // Propriedades finais do documento
        body.Append(new SectionProperties());

        return body;
    }

    private static Paragraph CriarTituloTema(string titulo)
    {
        return new Paragraph(
            new ParagraphProperties(
                new Justification { Val = JustificationValues.Center },
                 new SpacingBetweenLines
                 {
                     After = "400",
                     Before = "400"
                 }
            ),

            new Run(
                new RunProperties(new Bold()),
                new Text(titulo)
            )
        );
    }
}

