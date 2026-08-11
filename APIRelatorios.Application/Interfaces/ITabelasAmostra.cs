using APIRelatorios.Application.Contracts.DTOs;

namespace APIRelatorios.Application.Interfaces;

public interface ITabelasAmostra
{
    Task<byte[]> BuildAmostraAsync(IEnumerable<DadosRelatorioDTO> dto);
}
