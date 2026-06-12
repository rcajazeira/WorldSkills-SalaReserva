namespace SalaReservaApi.Models;

public class Reserva
{
    public int Id { get; set; }
    public int SalaId { get; set; }
    public Sala? Sala { get; set; }
    
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string Responsavel { get; set; } = string.Empty;
}