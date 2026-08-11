using APIRelatorios.Application.Contracts.DTOs;

namespace APIRelatorios.Dommain.Interfaces.Services;

public interface ITabelasFiscalizacao
{
    Task<byte[]> BuildAsync(IEnumerable<DadosRelatorioDTO> dto);
}
