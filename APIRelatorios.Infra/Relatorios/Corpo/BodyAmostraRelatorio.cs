using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Infra.Relatorios.Componentes;
using APIRelatorios.Infra.Relatorios.Context;
using DocumentFormat.OpenXml.Wordprocessing;

namespace APIRelatorios.Infra.Relatorios.Corpo;

public class BodyAmostraRelatorio
{
    internal static Body CriarTabelasAmostra(
                RelatorioContext ctx,
                IDictionary<string, List<DadosRelatorioDTO>> dadosPorTema
        )
    {
        var body = new Body();

        // Inicia Loop para declarar as variaveis para criar o Arquivo docx
        foreach (var grupo in dadosPorTema)
        {
            body.Append(
                CriarTituloTema(grupo.Key)
            );

            // Pega apenas UM registro para a tabela principal
            var registroPrincipal = grupo.Value.First();

            body.Append(
                RowComponentesAmostra.CriarTabela(
                    new List<DadosRelatorioDTO>
                    {
                        registroPrincipal
                    }
                )
            );

            // Aqui envia TODOS os registros,
            // portanto todas as imagens do grupo
            body.Append(
                RowImagesComponentes.CriarTabelasImagem(
                    ctx,
                    grupo.Value
                )
            );
        }

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
