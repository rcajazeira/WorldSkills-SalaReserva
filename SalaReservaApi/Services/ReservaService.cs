using Microsoft.EntityFrameworkCore;
using SalaReservaApi.Data;

namespace SalaReservaApi.Services;

public class ReservaService : IReservaService
{
    private readonly AppDbContext _context;

    // Injeção de dependência: o Service pede o banco de dados para funcionar
    public ReservaService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> IsSalaDisponivel(int salaId, DateTime inicio, DateTime fim, int? reservaIdIgnorar = null)
    {
        // Busca reservas para a mesma sala que NÃO são a própria reserva (no caso de edição)
        var reservasExistentes = await _context.Reservas
            .Where(r => r.SalaId == salaId && r.Id != reservaIdIgnorar)
            .ToListAsync();

        // LÓGICA DE CONFLITO (A mágica acontece aqui!)
        // Duas linhas do tempo [A, B] e [C, D] se sobrepõem se: A < D E C < B
        foreach (var reserva in reservasExistentes)
        {
            bool haConflito = inicio < reserva.DataFim && fim > reserva.DataInicio;
            if (haConflito)
            {
                return false; // Achou um conflito, a sala NÃO está disponível
            }
        }

        return true; // Passou por todas, não há conflitos
    }
}