using Cervejaria.Models;

namespace Cervejaria.Repositories
{
    public interface ICervejaRepository
    {
        Task<IEnumerable<Cerveja>> Get();
        Task<Cerveja> Get(int id);
        Task<Cerveja> Create(Cerveja cerveja);
        Task Update(Cerveja cerveja);
        Task Delete(int id);
    }
}
