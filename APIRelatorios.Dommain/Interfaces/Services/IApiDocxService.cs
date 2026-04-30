using APIRelatorios.Application.Contracts.DTOs;

namespace APIRelatorios.Dommain.Interfaces.Services;

public interface IApiDocxService
{
    Task<byte[]> emergencial(EmergencialDTO dto);
}
