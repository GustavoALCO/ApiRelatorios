using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Application.Contracts.DTOs;

public readonly record struct DadosRelatorioDTO
(
    // Bytes da imagem baixada da azure storage
    byte[] Foto,

    // Alimentador Obtido pela rota ou substituindo na Evidencia
    string Alimentador,

    // Descrição da evidencia
    string Dsc,

    // Identificação do poste
    string Identificação,

    // Dados como Rua e CEP
    string Localização,

    // Numero da imagem composta pela equipe selecionada + indice da imagem
    string NumeroImagem,

    // Enum para o Tema para atribuir as listas 
    TemaFiscalizacao Tema
);