namespace SalaReservaApi.Services;

public interface IReservaService
{
    Task<bool> IsSalaDisponivel(int salaId, DateTime inicio, DateTime fim, int? reservaIdIgnorar = null);
}