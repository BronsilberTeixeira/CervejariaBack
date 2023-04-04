using Cervejaria.Models;
using Cervejaria.Repositories;
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

        [HttpGet]
        public async Task<IEnumerable<Cerveja>> GetCerveja()
        {
            return await _cervejaRepository.Get();
        }
        [HttpGet("{id}")]

        public async Task<ActionResult<Cerveja>> GetCervejas(int id)
        {
            return await _cervejaRepository.Get(id);
        }

        [HttpPost]
        public async Task<ActionResult<Cerveja>> PostCerveja([FromBody] Cerveja cerveja)
        {
            var newBook = await _cervejaRepository.Create(cerveja);
            return cerveja;
        }

        [HttpDelete("{id}")]
        public  async Task<ActionResult<Cerveja>> Delete(int id)
        {
            var cervejaDelete = await _cervejaRepository.Get(id);
            if(cervejaDelete != null)
            {
                await _cervejaRepository.Delete(cervejaDelete.id);
                return NoContent();
            }
            return NotFound();
        }

        [HttpPut]
        public async Task<ActionResult<Cerveja>> PutCerveja(int id, [FromBody] Cerveja cerveja)
        {
            if(id == cerveja.id)
            {
                await _cervejaRepository.Update(cerveja);
                return NoContent();
            }
            return NotFound(); 
        }
    }
}



