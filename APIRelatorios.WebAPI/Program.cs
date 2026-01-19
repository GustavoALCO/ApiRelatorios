using APIRelatorios.Application.Settings;
using ChatApplication.Application.Settings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<DBSettings>()
    .Bind(builder.Configuration.GetRequiredSection("ConnectionStrings"))
    .ValidateDataAnnotations();
builder.Services.AddOptions<BlobSettings>()
    .Bind(builder.Configuration.GetRequiredSection("BlobConnection"))
    .ValidateDataAnnotations();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); // ✅ UMA ÚNICA VEZ, aqui

app.Run();
