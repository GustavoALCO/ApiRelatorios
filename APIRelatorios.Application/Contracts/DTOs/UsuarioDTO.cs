namespace APIRelatorios.Application.Contracts.DTOs;

public readonly record struct UsuarioDTO
(
     int UserId,

     string Login ,

     bool IsAdmin
);
