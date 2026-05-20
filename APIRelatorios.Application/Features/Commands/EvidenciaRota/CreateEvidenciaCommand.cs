using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Enuns;

namespace APIRelatorios.Application.Features.Commands.Images;

public record struct CreateEvidenciaCommand
(
Guid evidenciaId,

Guid rotaID,

int fiscalId,

int temaFiscalizacao,

List<int> subTemaFiscalizacao,

string? Identificacao,

string? Alimentador,

string? Descricao,

List<string> Base64,

string? Endereco,

string? Cidade,

double Latitude,

double Longitude,

DateTime Horario,

bool Emergencial
);
