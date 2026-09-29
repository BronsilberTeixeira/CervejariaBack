using Cervejaria.Context;
using Cervejaria.Models;
using Microsoft.EntityFrameworkCore;
using Cervejaria.Extensions;

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

        public async Task<List<Cerveja>> PegarCervejaFiltrada(CervejaFiltroDTO f)
        {
            var query = _context.Cervejas
                .WhereIf(!string.IsNullOrWhiteSpace(f.Nome), c => c.nome.Contains(f.Nome!))
                .WhereIf(!string.IsNullOrWhiteSpace(f.Tipo), c => c.tipo == f.Tipo)
                .WhereIf(f.PrecoMinimo > 0, c => c.preco >= f.PrecoMinimo!.Value)
                .WhereIf(f.PrecoMaximo > 0, c => c.preco <= f.PrecoMaximo!.Value);

            query = f.OrdenarPor?.ToLower() switch
            {
                "preco" => query.OrderBy(c => c.preco).ThenBy(c => c.id),
                "nome" => query.OrderBy(c => c.nome).ThenBy(c => c.id),
                _ => query.OrderBy(c => c.nome).ThenBy(c => c.id)
            };

            return await query
                .Skip((f.Page - 1) * f.PageSize)
                .Take(f.PageSize)
                .ToListAsync<Cerveja>();
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
