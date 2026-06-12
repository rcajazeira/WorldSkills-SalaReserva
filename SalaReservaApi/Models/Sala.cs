using System.Text.Json.Serialization;

namespace SalaReservaApi.Models;

public class Sala
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Capacidade { get; set; }
    
    [JsonIgnore]
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}