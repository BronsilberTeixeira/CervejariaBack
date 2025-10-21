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
        public async Task<Cerveja> Create(Cerveja cerveja)
        {
            _context.Cervejas.Add(cerveja);
            await _context.SaveChangesAsync();
            return cerveja;
        }

        public async Task Delete(int id)
        {
            var cervejaDelete = await _context.Cervejas.FindAsync(id);
            _context.Cervejas.Remove(cervejaDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Cerveja>> Get()
        {
            return await _context.Cervejas.ToListAsync();
        }

        public async Task<Cerveja> Get(int id)
        {
            return await _context.Cervejas.FindAsync(id);
        }

        public async Task Update(Cerveja cerveja)
        {
           _context.Entry(cerveja).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
