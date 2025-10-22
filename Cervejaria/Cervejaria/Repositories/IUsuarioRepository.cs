using Cervejaria.Models;

namespace Cervejaria.Repositories
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> PegarTodosUsuarios();
        Task<Usuario> ValidarUsuario(string email, string senha);
        Task<Usuario> PegarUsuarioPorEmail(string email);
        Task<Usuario> PegarUsuarioPorId(int id);
        Task<Usuario> CriarUsuario(Usuario usuario);
        Task<Usuario> EditarUsuario(Usuario usuario);
        Task<Usuario> ExcluirUsuario(int id);
    }
}
