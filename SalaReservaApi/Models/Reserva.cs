namespace SalaReservaApi.Models;

public class Reserva
{
    public int Id { get; set; }
    public int SalaId { get; set; } // Chave estrangeira: liga a reserva à sala
    public Sala? Sala { get; set; } // Navegação para o objeto Sala
    
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string Responsavel { get; set; } = string.Empty; // Nome de quem reservou
}