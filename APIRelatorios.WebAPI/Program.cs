using APIRelatorios.Application.Settings;
using APIRelatorios.Infra.Database;
using APIRelatorios.IOC;
using ChatApplication.Application.Settings;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Kestrel
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
});

// Configurações
builder.Services.AddOptions<BlobSettings>()
    .Bind(builder.Configuration.GetRequiredSection("BlobConnection"))
    .ValidateDataAnnotations();

builder.Services.AddOptions<JWTSettings>()
    .Bind(builder.Configuration.GetRequiredSection("Jwt"))
    .ValidateDataAnnotations();

builder.Services.AddOptions<AzureMapsSettings>()
    .Bind(builder.Configuration.GetRequiredSection("Azurekey"))
    .ValidateDataAnnotations();

// JWT Authentication
builder.Services.Authentication(builder.Configuration);

// Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.DeclareFluentValidate();

// Swagger com JWT
builder.Services.AddSwagger();

// Infraestrutura, interfaces e handlers
builder.Services.AddInfra(builder.Configuration);
builder.Services.DeclareInterfaces();
builder.Services.DeclareInterfacesServices();
builder.Services.DeclareHandlerAplication();

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Relatorio v1");
});

// 🔹 Autenticação + Autorização
app.UseAuthentication();
app.UseAuthorization();

// Map Controllers
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<DatabaseContext>();

    db.Database.Migrate();

    await DependencyInjection.SeedAsync(services); 
}

app.Run();