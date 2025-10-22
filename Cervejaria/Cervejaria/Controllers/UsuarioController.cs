using Cervejaria.Models;
using Cervejaria.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Cervejaria.Controllers {     
    
    [Route("Api/[controller]/Usuario")]
    [ApiController]

    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<Usuario>> GetUsuario()
        {
            return await _usuarioRepository.PegarTodosUsuarios();
        }

        [HttpGet("pegarUsuario/{email}/{senha}")]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> GetUsuario(string email, string senha)
        {
            var senhaCriptografada = HashSenha(senha);

            // 🔍 Busca o usuário no banco
            var usuario = await _usuarioRepository.ValidarUsuario(email, senhaCriptografada);

            if (usuario == null) 
            {
                // ❌ Usuário não encontrado ou senha incorreta
                return Unauthorized(new { Mensagem = "Nome ou senha inválidos." });
            }

            // ✅ Gera o token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("chave-super-secreta-12345"); // use uma chave forte e segura

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.Name, usuario.Nome),
            new Claim("UserId", usuario.Id.ToString())
        }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // 🔙 Retorna o token e informações básicas do usuário
            return Ok(new
            {
                Token = tokenString,
                Usuario = new
                {
                    usuario.Id,
                    usuario.Nome
                }
            });
        }

        [HttpPost("CriarUsuario")]
        [AllowAnonymous]
        public async Task<ActionResult<Usuario>> PostUsuario([FromBody] Usuario usuario)
        {
            // Criptografa a senha antes de salvar
            var jaExiste = await _usuarioRepository.PegarUsuarioPorEmail(usuario.Email);
            if (jaExiste != null)
            {
                return BadRequest(new { Mensagem = "Já existe um usuário com este email." });
            }

            usuario.Senha = HashSenha(usuario.Senha);
            var novoUsuario = await _usuarioRepository.CriarUsuario(usuario);
            return CreatedAtAction(nameof(GetUsuario), new { id = novoUsuario.Id }, novoUsuario);
        }

        [HttpPut("EdiatrUsuario")]
        public async Task<ActionResult<Usuario>> PutUsuario([FromBody] Usuario usuario)
        {
            var usuarioExistente = await _usuarioRepository.PegarUsuarioPorId(usuario.Id);
            if (usuarioExistente == null)
            {
                return NotFound(new { Mensagem = "Usuário não encontrado." });
            }
            // Atualiza os campos necessários
            usuarioExistente.Nome = usuario.Nome;
            usuarioExistente.Email = usuario.Email;
            // Se a senha foi alterada, criptografa a nova senha
            if (!string.IsNullOrEmpty(usuario.Senha))
            {
                usuarioExistente.Senha = HashSenha(usuario.Senha);
            }
            await _usuarioRepository.EditarUsuario(usuarioExistente);
            return NoContent();
        }

        [HttpDelete("ExcluirUsuario")]
        public async Task<ActionResult> ExcluirUsuario(string email, string senha)
        {
            var senhaCriptografada = HashSenha(senha);
            var usuarioExistente = await _usuarioRepository.ValidarUsuario(email, senhaCriptografada);
            if (usuarioExistente == null)
            {
                return BadRequest(new { Mensagem = "E-mail e/ou senha estão incorretos" });
            }
            await _usuarioRepository.ExcluirUsuario(usuarioExistente.Id);
            return NoContent();
        }

        private string HashSenha(string senha)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(senha));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}
