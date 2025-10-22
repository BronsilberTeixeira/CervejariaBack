using Cervejaria.Models;

namespace Cervejaria.Repositories
{
    public interface ICervejaRepository
    {
        Task<IEnumerable<Cerveja>> PegarTodasCerverjas();
        Task<Cerveja> PegarCervejaId(int id);
        Task<Cerveja> CriarCerveja(Cerveja cerveja);
        Task EditarCerveja(Cerveja cerveja);
        Task ExcluirCerveja(int id);
    }
}
