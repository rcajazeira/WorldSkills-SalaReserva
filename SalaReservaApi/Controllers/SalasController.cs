using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalaReservaApi.Data;
using SalaReservaApi.Models;
using SalaReservaApi.Services;

namespace SalaReservaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalasController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IReservaService _reservaService;

    public SalasController(AppDbContext context, IReservaService reservaService)
    {
        _context = context;
        _reservaService = reservaService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sala>>> GetSalas()
    {
        return await _context.Salas.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sala>> GetSala(int id)
    {
        var sala = await _context.Salas.FindAsync(id);
        if (sala == null) return NotFound();
        return sala;
    }

    [HttpPost]
    public async Task<ActionResult<Sala>> PostSala(Sala sala)
    {
        _context.Salas.Add(sala);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSala), new { id = sala.Id }, sala);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutSala(int id, Sala sala)
    {
        if (id != sala.Id) return BadRequest();
        _context.Entry(sala).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSala(int id)
    {
        var sala = await _context.Salas.FindAsync(id);
        if (sala == null) return NotFound();
        _context.Salas.Remove(sala);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("disponiveis")]
    public async Task<ActionResult<IEnumerable<Sala>>> GetSalasDisponiveis(
        [FromQuery] DateTime dataInicio, 
        [FromQuery] DateTime dataFim)
    {
        var todasSalas = await _context.Salas.ToListAsync();
        var salasLivres = new List<Sala>();

        foreach (var sala in todasSalas)
        {
            bool disponivel = await _reservaService.IsSalaDisponivel(sala.Id, dataInicio, dataFim);
            if (disponivel)
            {
                salasLivres.Add(sala);
            }
        }

        return salasLivres;
    }
}