namespace APIRelatorios.Application.Contracts.DTOs;

public record struct AmostraCsvDto
(
     string? SeqISA ,
     string? SeqBaseFisica ,
     double? VlrBase ,
     string? DescricaoTUC ,
     string? DescricaoTec ,
     string? ODIEngenharia ,
     string? Instalacao ,
     string? Endereco ,
     string? Municipio ,
     double Latitude ,
     double Longitude ,
     string? TUC1 ,
     string? TUC2 ,
     string? TUC3 ,
     string? TUC4 ,
     string? TUC5 ,
     string? TUC6 ,
     string? NumSerie ,
     string? PosicaoOperativa ,
     string? Equipamento 
);
