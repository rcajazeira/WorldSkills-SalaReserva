namespace SalaReservaApi.Models;

public class Sala
{
    public int Id { get; set; } // O banco vai gerar esse número automaticamente
    public string Nome { get; set; } = string.Empty; // Ex: "Sala de Reuniões A"
    public int Capacidade { get; set; } // Ex: 10 pessoas
    
    // Relacionamento: Uma Sala tem Várias Reservas
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}