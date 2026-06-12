using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalaReservaApi.Data;
using SalaReservaApi.Models;

namespace SalaReservaApi.Controllers;

[ApiController]
[Route("api/[controller]")] // A URL será /api/salas
public class SalasController : ControllerBase
{
    private readonly AppDbContext _context;

    public SalasController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/salas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sala>>> GetSalas()
    {
        return await _context.Salas.ToListAsync();
    }

    // GET: api/salas/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Sala>> GetSala(int id)
    {
        var sala = await _context.Salas.FindAsync(id);
        if (sala == null) return NotFound(); // HTTP 404
        return sala;
    }

    // POST: api/salas
    [HttpPost]
    public async Task<ActionResult<Sala>> PostSala(Sala sala)
    {
        _context.Salas.Add(sala);
        await _context.SaveChangesAsync(); // Salva no banco
        
        // Retorna HTTP 201 (Created) e a URL do novo recurso
        return CreatedAtAction(nameof(GetSala), new { id = sala.Id }, sala); 
    }

    // PUT: api/salas/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSala(int id, Sala sala)
    {
        if (id != sala.Id) return BadRequest(); // HTTP 400 (Erro do cliente)

        _context.Entry(sala).State = EntityState.Modified; // Marca como alterado
        await _context.SaveChangesAsync();

        return NoContent(); // HTTP 204 (Sucesso, mas sem corpo de resposta)
    }

    // DELETE: api/salas/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSala(int id)
    {
        var sala = await _context.Salas.FindAsync(id);
        if (sala == null) return NotFound();

        _context.Salas.Remove(sala);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}