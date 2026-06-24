using APIRelatorios.Application.Abstractions.Messaging;
using APIRelatorios.Dommain.Entities;
using APIRelatorios.Dommain.Interfaces.Services;
using APIRelatorios.Dommain.Interfaces.User;
using Microsoft.Extensions.Logging;

namespace APIRelatorios.Application.Features.Commands.User.Handlers;

public class CreateUserHandler
    : ICommandHandler<CreateUsuarioCommand>
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

    public async Task Handle(CreateUsuarioCommand createuser, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando processo de criação de usuário para {Nome} {Sobrenome}", createuser.nome, createuser.sobreNome);
        // Coleta o Ultimo nome do usuário para criar o login
        string ultimoNome = createuser.sobreNome.Split(' ').Last();

        _logger.LogInformation("Último nome extraído: {UltimoNome}", ultimoNome);

        // Cria o login do usuário utilizando o nome e o ultimo nome
        var login = $"{createuser.nome.ToLower()}.{ultimoNome.ToLower()}";

        _logger.LogInformation("Login inicial gerado: {Login}", login);


        _logger.LogInformation("Verificando se o login {Login} já existe no banco de dados.", login);
        var userExist = await _query.BuscarTodosFiscalLogin(login);

        if (userExist.Any())
        {   
            _logger.LogInformation("Login {Login} já existe. Gerando um novo login.", login);

            var count = userExist.Count;

            login = $"{login}{count}";

            _logger.LogInformation("Novo login gerado: {Login}", login);
        }

        Dommain.Entities.User user = new();

        _logger.LogInformation("Gerando hash da senha para o usuário {Login}.", login);
        var salt = _passwordHasher.GenerateHash();

        var hashPassword = _passwordHasher.HashPassword(createuser.senha, salt);

        _logger.LogInformation("Criando usuário com login {Login}.", login);
        user.CreateUser(login, createuser.nome, createuser.sobreNome, hashPassword, salt, createuser.isAdmin);
       
        await _commands.CreateUser(user);

        _logger.LogInformation("Usuário {Login} criado com sucesso.", login);
    }
}
