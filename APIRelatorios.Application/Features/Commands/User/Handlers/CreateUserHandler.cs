using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Services;
using APIRelatorios.Dommain.Interfaces.User;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.User.Handlers;

public class CreateUserHandler
{
    private readonly IUserCommands _commands;

    private readonly IUserQuery _query;

    private readonly IPasswordHasher _passwordHasher;

    private readonly ILogger<CreateUserHandler> _logger;

    public CreateUserHandler(IUserQuery query, IUserCommands commands, IPasswordHasher passwordHasher, ILogger<CreateUserHandler> logger)
    {
        _commands = commands;
        _query = query;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task Handler(CreateUsuarioCommand createuser)
    {

        // Coleta o Ultimo nome do usuário para criar o login
        string ultimoNome = createuser.sobreNome.Split(' ').Last();

        // Cria o login do usuário utilizando o nome e o ultimo nome
        var login = $"{createuser.nome.ToLower()}.{ultimoNome.ToLower()}";

        var userExist = await _query.BuscarTodosFiscalLogin(login);

        if (userExist.Any())
        {   
            _logger.LogInformation("Login {Login} já existe. Gerando um novo login.", login);

            var count = userExist.Count;

            login = $"{login}{count}";

            _logger.LogInformation("Novo login gerado: {Login}", login);
        }

        Dommain.Entities.User user = new();

        //Gera um salt para ser armazenado no banco de dados
        var salt = _passwordHasher.GenerateHash();

        var hashPassword = _passwordHasher.HashPassword(createuser.senha, salt);

        user.CreateUser(login, createuser.nome, createuser.sobreNome, hashPassword, salt, createuser.isAdmin);
        

        await _commands.CreateUser(user);
    }
}
