using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalaReservaApi.Data;
using SalaReservaApi.Models;
using SalaReservaApi.Services;

namespace SalaReservaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IReservaService _reservaService; // Injetando nosso cérebro!

    public ReservasController(AppDbContext context, IReservaService reservaService)
    {
        _context = context;
        _reservaService = reservaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
    {
        // Include traz os dados da Sala junto com a Reserva
        return await _context.Reservas.Include(r => r.Sala).ToListAsync(); 
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Reserva>> GetReserva(int id)
    {
        var reserva = await _context.Reservas.Include(r => r.Sala).FirstOrDefaultAsync(r => r.Id == id);
        if (reserva == null) return NotFound();
        return reserva;
    }

    [HttpPost]
    public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
    {
        // 1. Validar conflito ANTES de salvar
        bool disponivel = await _reservaService.IsSalaDisponivel(
            reserva.SalaId, reserva.DataInicio, reserva.DataFim);

        if (!disponivel)
        {
            return BadRequest(new { mensagem = "Conflito de horário! A sala já está reservada neste período." });
        }

        // 2. Se não houver conflito, salva
        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetReserva), new { id = reserva.Id }, reserva);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutReserva(int id, Reserva reserva)
    {
        if (id != reserva.Id) return BadRequest();

        // 1. Validar conflito, IGNORANDO a própria reserva que estamos editando
        bool disponivel = await _reservaService.IsSalaDisponivel(
            reserva.SalaId, reserva.DataInicio, reserva.DataFim, id);

        if (!disponivel)
        {
            return BadRequest(new { mensagem = "Conflito de horário!" });
        }

        _context.Entry(reserva).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReserva(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null) return NotFound();

        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}