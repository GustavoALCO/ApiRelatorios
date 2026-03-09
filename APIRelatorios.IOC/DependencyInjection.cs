using APIRelatorios.Application.Features.Commands.Images.Handler;
using APIRelatorios.Application.Features.Commands.Rota.Handler;
using APIRelatorios.Application.Features.Commands.User.Handlers;
using APIRelatorios.Application.Features.Querys.EvidenciaRota.Handler;
using APIRelatorios.Application.Features.Querys.Rota.Handler;
using APIRelatorios.Application.Features.Querys.User.Handler;
using APIRelatorios.Application.Interfaces;
using APIRelatorios.Application.Services;
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
using APIRelatorios.Infra.Security;
using ChatApplication.Application.Interfaces;
using ChatApplication.Application.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Reflection;
using System.Text;

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

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Relatorio",
                Version = "v1"
            });

            // Define esquema de segurança Bearer JWT
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Digite: Bearer {token}"
            });

            // Adiciona requisito usando delegate
            c.AddSecurityRequirement(document =>
            {
                return new OpenApiSecurityRequirement
                {
                    { new OpenApiSecuritySchemeReference("Bearer"), new List<string>() }
                };
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

        services.AddScoped<IValidateBase64, ValidateBase64>();

        services.AddScoped<IValidateIds, ValidateIds>();

        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }

    public static IServiceCollection DeclareHandlerAplication(this IServiceCollection services)
    {
        services.AddScoped<CreateImageHandler>();
        services.AddScoped<DeleteImageHandler>();
        services.AddScoped<UpdateDescricaoImageHandler>();
        services.AddScoped<BuscarEvidenciaPorIdHandler>();
        services.AddScoped<BuscarTodasAsEvidenciasRotaHandler>();
        services.AddScoped<BuscarRotaFiltersHandler>();
        services.AddScoped<BuscarRotaIdHandler>();

        services.AddScoped<AddFiscalRotaHandler>();
        services.AddScoped<CreateRotaHandler>();
        services.AddScoped<DeleteRotaHandler>();
        services.AddScoped<RemoveFiscalRotaHandler>();
        services.AddScoped<UpdateNomeRotaHandler>();
        services.AddScoped<UpdatePasswordHandler>();
        services.AddScoped<CreateRelatorioHandler>();

        services.AddScoped<LoginHandler>();
        services.AddScoped<DeleteUsuarioHandler>();
        services.AddScoped<UpdateUsuarioHandler>();
        services.AddScoped<CreateUserHandler>();
        services.AddScoped<BuscarTodosUsuariosHandler>();

        return services;
    }

    public static IServiceCollection DeclareFluentValidate(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.Load("APIRelatorios.Application"));

        return services;
    }

    public static IServiceCollection Authentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("Jwt");
        var jwtSettings = jwtSection.Get<JWTSettings>();

        services.Configure<JWTSettings>(jwtSection); // Disponibiliza o IOptions<JWTSettings> para injeção

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,

                ValidateAudience = true,
                ValidAudiences = jwtSettings.Audience, 

                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                ValidateIssuerSigningKey = true,

                RoleClaimType = "Role" // usa o claim "Role" no token
            };
        });

        return services;
    }
}
