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

        [HttpGet("PegarCervejas")]
        [AllowAnonymous]
        public async Task<IEnumerable<Cerveja>> GetCerveja()
        {
            return await _cervejaRepository.PegarTodasCerverjas();
        }

        [HttpGet("PegarCerveja/{id}")]
        public async Task<ActionResult<Cerveja>> GetCervejas(int id)
        {
            return await _cervejaRepository.PegarCervejaId(id);
        }

        [HttpPost("CriarCerveja")]
        [AllowAnonymous]
        public async Task<ActionResult<Cerveja>> PostCerveja([FromBody] Cerveja cerveja)
        {
            var novaCerveja = await _cervejaRepository.CriarCerveja(cerveja);
            return cerveja;
        }

        [HttpDelete("ExcluirCerveja/{id}")]
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

        [HttpPut("EditarCerveja")]
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



