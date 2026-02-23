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

        public async Task<List<Cerveja>> PegarCervejaFiltrada(CervejaFiltroDTO cervejaFiltro)
        {
            IQueryable<Cerveja> query = _context.Cervejas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(cervejaFiltro.Nome))
            {
                query = query.Where(c => c.nome.Contains(cervejaFiltro.Nome));
            }
            else if(!string.IsNullOrWhiteSpace(cervejaFiltro.tipo))
            {
                query = query.Where(c => c.tipo == cervejaFiltro.tipo);
            }
            else if(cervejaFiltro.precoMinimo.HasValue && cervejaFiltro.precoMinimo > 0)
            {
                query = query.Where(c => c.preco >= cervejaFiltro.precoMinimo.Value);
            }
            else if (cervejaFiltro.precoMaximo.HasValue && cervejaFiltro.precoMaximo > 0)
            {
                query = query.Where(c => c.preco <= cervejaFiltro.precoMaximo.Value);
            }
            if (!string.IsNullOrEmpty(cervejaFiltro.OrdenarPor))
            {
                query = cervejaFiltro.OrdenarPor.ToLower() switch
                {
                    "preco" => query.OrderBy(c => c.preco),
                    "nome" => query.OrderBy(c => c.nome),
                    _ => query
                };
            }

            query = query
                .Skip((cervejaFiltro.Page - 1) * cervejaFiltro.PageSize)
                .Take(  cervejaFiltro.PageSize);

            return await query.ToListAsync();
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
