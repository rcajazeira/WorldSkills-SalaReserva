using Microsoft.EntityFrameworkCore;
using SalaReservaApi.Data;
using SalaReservaApi.Services;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Adiciona controllers e configura JSON para ignorar loops de referência
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddOpenApi();

// Registro do DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro do Service
builder.Services.AddScoped<IReservaService, ReservaService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Habilita arquivos estáticos (frontend)
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();