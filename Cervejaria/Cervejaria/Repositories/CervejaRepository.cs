using Cervejaria.Context;
using Cervejaria.Models;
using Microsoft.EntityFrameworkCore;

namespace Cervejaria.Repositories
{
    public class CervejaRepository : ICervejaRepository
    {
        public readonly CervejariaContext _context;

        public CervejaRepository (CervejariaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cerveja>> PegarTodasCerverjas()
        {
            return await _context.Cervejas.ToListAsync();
        }

        public async Task<Cerveja> CriarCerveja(Cerveja cerveja)
        {
            _context.Cervejas.Add(cerveja);
            await _context.SaveChangesAsync();
            return cerveja;
        }

        public async Task ExcluirCerveja(int id)
        {
            var cervejaDelete = await _context.Cervejas.FindAsync(id);
            _context.Cervejas.Remove(cervejaDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<Cerveja> PegarCervejaId(int id)
        {
            return await _context.Cervejas.FindAsync(id);
        }

        public async Task EditarCerveja(Cerveja cerveja)
        {
           _context.Entry(cerveja).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
