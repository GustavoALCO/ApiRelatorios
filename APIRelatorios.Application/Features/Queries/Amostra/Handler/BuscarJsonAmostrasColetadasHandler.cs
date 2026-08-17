using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Application.Contracts.DTOs;
using APIRelatorios.Domain.Interfaces.Amostra;
using ClosedXML.Excel;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Queries.Amostra.Handler;

public class BuscarJsonAmostrasColetadasHandler
    : IQueryHandler<BuscarJsonAmostrasColetadasQuery, byte[]>
{
    private readonly IAmostraQuery _amostraQuery;
    private readonly ILogger<BuscarJsonAmostrasColetadasHandler> _logger;

    public BuscarJsonAmostrasColetadasHandler(
        ILogger<BuscarJsonAmostrasColetadasHandler> logger,
        IAmostraQuery amostraQuery)
    {
        _logger = logger;
        _amostraQuery = amostraQuery;
    }

    public async Task<byte[]> Handle(
        BuscarJsonAmostrasColetadasQuery query,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Iniciando a execução do handler BuscarJsonAmostrasColetadasHandler"
        );

        var amostras = await _amostraQuery.GetAmostraCheck(query.idrota);

        _logger.LogInformation(
            "Foram encontradas {Quantidade} amostras",
            amostras.Count()
        );

        _logger.LogInformation(
            "Iniciando processo de geração do arquivo Excel"
        );

        using var workbook = new XLWorkbook();

        var worksheet = workbook.Worksheets.Add("Amostras");

        // ==============================
        // CABEÇALHO
        // ==============================

        worksheet.Cell(1, 1).Value = "Id";
        worksheet.Cell(1, 2).Value = "RotaId";
        worksheet.Cell(1, 3).Value = "SeqISA";
        worksheet.Cell(1, 4).Value = "SeqBaseFisica";
        worksheet.Cell(1, 5).Value = "VlrBase";
        worksheet.Cell(1, 6).Value = "DescricaoTUC";
        worksheet.Cell(1, 7).Value = "DescricaoTec";
        worksheet.Cell(1, 8).Value = "ODIEngenharia";
        worksheet.Cell(1, 9).Value = "Instalacao";
        worksheet.Cell(1, 10).Value = "Endereco";
        worksheet.Cell(1, 11).Value = "Municipio";
        worksheet.Cell(1, 12).Value = "Latitude";
        worksheet.Cell(1, 13).Value = "Longitude";
        worksheet.Cell(1, 14).Value = "TUC1";
        worksheet.Cell(1, 15).Value = "TUC2";
        worksheet.Cell(1, 16).Value = "TUC3";
        worksheet.Cell(1, 17).Value = "TUC4";
        worksheet.Cell(1, 18).Value = "TUC5";
        worksheet.Cell(1, 19).Value = "TUC6";
        worksheet.Cell(1, 20).Value = "NumSerie";
        worksheet.Cell(1, 21).Value = "PosicaoOperativa";
        worksheet.Cell(1, 22).Value = "Equipamento";
        worksheet.Cell(1, 23).Value = "DataFabricacao";
        worksheet.Cell(1, 24).Value = "Observacao";
        worksheet.Cell(1, 25).Value = "Fotos";
        worksheet.Cell(1, 26).Value = "Sincronizado";

        // ==============================
        // DADOS
        // ==============================

        int linha = 2;

        foreach (var item in amostras)
        {
            _logger.LogInformation(
                "Adicionando amostra com Id: {Id}",
                item.Id
            );

            worksheet.Cell(linha, 1).Value = item.Id.ToString();
            worksheet.Cell(linha, 2).Value = item.RotaId.ToString();
            worksheet.Cell(linha, 3).Value = item.SeqISA;
            worksheet.Cell(linha, 4).Value = item.SeqBaseFisica;
            worksheet.Cell(linha, 5).Value = item.VlrBase;
            worksheet.Cell(linha, 6).Value = item.DescricaoTUC;
            worksheet.Cell(linha, 7).Value = item.DescricaoTec;
            worksheet.Cell(linha, 8).Value = item.ODIEngenharia;
            worksheet.Cell(linha, 9).Value = item.Instalacao;
            worksheet.Cell(linha, 10).Value = item.Endereco;
            worksheet.Cell(linha, 11).Value = item.Municipio;
            worksheet.Cell(linha, 12).Value = item.Latitude;
            worksheet.Cell(linha, 13).Value = item.Longitude;
            worksheet.Cell(linha, 14).Value = item.TUC1;
            worksheet.Cell(linha, 15).Value = item.TUC2;
            worksheet.Cell(linha, 16).Value = item.TUC3;
            worksheet.Cell(linha, 17).Value = item.TUC4;
            worksheet.Cell(linha, 18).Value = item.TUC5;
            worksheet.Cell(linha, 19).Value = item.TUC6;
            worksheet.Cell(linha, 20).Value = item.NumSerie;
            worksheet.Cell(linha, 21).Value = item.PosicaoOperativa;
            worksheet.Cell(linha, 22).Value = item.Equipamento;
            worksheet.Cell(linha, 23).Value = item.DataFabricacao;
            worksheet.Cell(linha, 24).Value = item.Observacao;

            // Como Fotos provavelmente é uma coleção/array,
            // transforma em texto para não dar erro no Excel.
            worksheet.Cell(linha, 25).Value =
                item.Fotos != null
                    ? string.Join(", ", item.Fotos)
                    : "";

            worksheet.Cell(linha, 26).Value = item.Sincronizado;

            linha++;
        }

        // ==============================
        // FORMATAÇÃO
        // ==============================

        var cabecalho = worksheet.Range(1, 1, 1, 26);

        cabecalho.Style.Font.Bold = true;

        // Cria filtro no cabeçalho
        worksheet.RangeUsed()?.SetAutoFilter();

        // Congela a primeira linha
        worksheet.SheetView.FreezeRows(1);

        // Ajusta largura automaticamente
        worksheet.Columns().AdjustToContents();

        _logger.LogInformation(
            "Arquivo Excel gerado com sucesso"
        );

        using var stream = new MemoryStream();

        workbook.SaveAs(stream);

        return stream.ToArray();
    }
}