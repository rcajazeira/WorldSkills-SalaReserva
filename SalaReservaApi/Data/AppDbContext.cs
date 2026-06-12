using Microsoft.EntityFrameworkCore;
using SalaReservaApi.Models;

namespace SalaReservaApi.Data;

public class AppDbContext : DbContext
{
    // O construtor recebe as "configurações" e passa para a classe base
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Estas propriedades representam as tabelas no banco de dados
    public DbSet<Sala> Salas { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
}