using Cervejaria.Models;
using Microsoft.EntityFrameworkCore;

namespace Cervejaria.Context
{
    public class CervejariaContext : DbContext
    {
        public CervejariaContext(DbContextOptions<CervejariaContext> options) : base(options) { }

        public DbSet<Cerveja> Cervejas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}
 