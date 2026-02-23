using Cervejaria.Models;
using Cervejaria.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cervejaria.Controllers
{
    [Route("Api/[controller]/Cerveja")]
    [ApiController]

    public class CervejaController : ControllerBase
    {
        private readonly ICervejaRepository _cervejaRepository;
        public CervejaController(ICervejaRepository cervejaRepository)
        {
            _cervejaRepository = cervejaRepository;
        }

        [HttpGet("pegar-todas-cervejas")]
        [AllowAnonymous]
        public async Task<IEnumerable<Cerveja>> GetCerveja()
        {
            return await _cervejaRepository.PegarTodasCerverjas();
        }

        [HttpGet("pegar-cerveja/{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Cerveja>> GetCervejas(int id)
        {
            return await _cervejaRepository.PegarCervejaId(id);
        }

        [HttpPost("pegar-cervejas-filtro")]
        [AllowAnonymous]
        public async Task<ActionResult<Cerveja>> PostCerveja([FromBody] CervejaFiltroDTO filtroCerveja)
        {
            var cervejasFiltradas = await _cervejaRepository.PegarCervejaFiltrada(filtroCerveja);
            if(!cervejasFiltradas.Any())
            {
                return NotFound("Nenhuma cerveja encontrada!");
            }
            return Ok(cervejasFiltradas);
        }

        [HttpPost("criar-cerveja")]
        public async Task<ActionResult<Cerveja>> PostCerveja([FromBody] Cerveja cerveja)
        {
            var novaCerveja = await _cervejaRepository.CriarCerveja(cerveja);
            return cerveja;
        }

        [HttpDelete("excluir-cerveja/{id}")]
        public  async Task<ActionResult<Cerveja>> Delete(int id)
        {
            var cervejaDelete = await _cervejaRepository.PegarCervejaId(id);
            if(cervejaDelete != null)
            {
                await _cervejaRepository.ExcluirCerveja(cervejaDelete.id);
                return NoContent();
            }
            return NotFound();
        }

        [HttpPut("editar-cerveja")]
        public async Task<ActionResult<Cerveja>> PutCerveja(int id, [FromBody] Cerveja cerveja)
        {
            if(id == cerveja.id)
            {
                await _cervejaRepository.EditarCerveja(cerveja);
                return NoContent();
            }
            return NotFound(); 
        }
    }
}



