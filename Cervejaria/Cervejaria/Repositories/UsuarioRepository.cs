using Cervejaria.Context;
using Cervejaria.Models;
using Microsoft.EntityFrameworkCore;

namespace Cervejaria.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly CervejariaContext _context;
        public UsuarioRepository(CervejariaContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Usuario>> PegarTodosUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }
        public async Task<Usuario> ValidarUsuario(string email, string senha)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email && u.Senha == senha);
        }

        public async Task<Usuario> PegarUsuarioPorId(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> PegarUsuarioPorEmail(string email)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario> CriarUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> EditarUsuario(Usuario usuario)
        {
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> ExcluirUsuario(int id)
        {
            var usuarioDelete = await _context.Usuarios.FindAsync(id);
            _context.Usuarios.Remove(usuarioDelete);
            await _context.SaveChangesAsync();
            return usuarioDelete;
        }
    }
}
