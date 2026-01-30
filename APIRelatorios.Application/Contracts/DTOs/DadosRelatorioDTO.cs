using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Application.Contracts.DTOs;

public readonly record struct DadosRelatorioDTO
(
    // Bytes da imagem baixada da azure storage
    byte[] Foto,

    // Descrição formado por Alimentador + Dsc + Endereço + CEP
    string Dsc,

    // Numero da imagem composta pela equipe selecionada + indice da imagem
    string NumeroImagem,

    // Enum para o Tema para atribuir as listas 
    TemaFiscalizacao Tema
);