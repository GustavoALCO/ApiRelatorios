using APIRelatorios.Application.Settings;
using APIRelatorios.IOC;
using ChatApplication.Application.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(8080);
});

builder.Services.AddOptions<BlobSettings>()
    .Bind(builder.Configuration.GetRequiredSection("BlobConnection"))
    .ValidateDataAnnotations();
builder.Services.AddOptions<JWTSettings>()
    .Bind(builder.Configuration.GetRequiredSection("Jwt"))
    .ValidateDataAnnotations();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfra(builder.Configuration);

builder.Services.DeclareInterfaces();

builder.Services.DeclareInterfacesServices();

builder.Services.DeclareHandlerAplication();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers(); 

app.Run();
