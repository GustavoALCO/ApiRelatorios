using APIRelatorios.Application.Features.Commands.Images.Handler;
using APIRelatorios.Application.Features.Commands.Rota.Handler;
using APIRelatorios.Application.Features.Commands.User.Handlers;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Application.Settings;
using APIRelatorios.Dommain.Interfaces.Images;
using APIRelatorios.Dommain.Interfaces.Rota;
using APIRelatorios.Dommain.Interfaces.Services;
using APIRelatorios.Dommain.Interfaces.User;
using APIRelatorios.Infra.Auth;
using APIRelatorios.Infra.Database;
using APIRelatorios.Infra.Relatorios.Index;
using APIRelatorios.Infra.Repository.Images;
using APIRelatorios.Infra.Repository.Rota;
using APIRelatorios.Infra.Repository.User;
using APIRelatorios.Infra.Requets;
using ChatApplication.Application.Interfaces;
using ChatApplication.Application.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace APIRelatorios.IOC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDatabase(configuration);

        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<DBSettings>(
            configuration.GetSection("ConnectionStrings"));

        services.AddDbContext<DatabaseContext>((provider, options) =>
        {
            var config = provider
                .GetRequiredService<IOptions<DBSettings>>()
                .Value;

            var connectionString =
                $"Host={config.Host};" +
                $"Port={config.Port};" +
                $"Database={config.Database};" +
                $"Username={config.User};" +
                $"Password={config.Password}";

            options.UseNpgsql(
                connectionString,
                npgsql =>
                {
                    npgsql.MigrationsAssembly(
                        typeof(DatabaseContext).Assembly.FullName);
                });
        });

        return services;
    }

    public static IServiceCollection DeclareInterfaces(this IServiceCollection services)
    {
        services.AddScoped<IEvidenciaRotaCommands, EvidenciaRotaCommands>();
        services.AddScoped<IEvidenciaRotaQuery, EvidenciaRotaQuery>();

        services.AddScoped<IUserCommands, UserCommands>();
        services.AddScoped<IUserQuery, UserQuery>();

        services.AddScoped<IRotaCommands, RotaCommands>();
        services.AddScoped<IRotaQuery, RotaQuery>();

        return services;
    }

    public static IServiceCollection DeclareInterfacesServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenService, JWTTokenService>();

        services.AddScoped<ISavedImages, SavedImage>();

        services.AddScoped<IRelatorioDeIrregularidades, RelatorioDeIrregulariedades>();

        services.AddScoped<IBuscarByteImagem, BuscarByteImagem>();

        return services;
    }

    public static IServiceCollection DeclareHandlerAplication(this IServiceCollection services)
    {
        services.AddScoped<CreateImageHandler>();
        services.AddScoped<DeleteImageHandler>();
        services.AddScoped<UpdateDescricaoImageHandler>();

        services.AddScoped<AddFiscalRotaHandler>();
        services.AddScoped<CreateRotaHandler>();
        services.AddScoped<DeleteRotaHandler>();
        services.AddScoped<RemoveFiscalRotaHandler>();
        services.AddScoped<UpdateNomeRotaHandler>();
        services.AddScoped<CreateRelatorioHandler>();

        services.AddScoped<LoginHandler>();
        services.AddScoped<DeleteUsuarioHandler>();
        services.AddScoped<UpdateUsuarioHandler>();
        services.AddScoped<CreateUserHandler>();

        return services;
    }
}
