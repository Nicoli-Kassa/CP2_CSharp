using CP2_CSharp.Data;
using CP2_CSharp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CP2_CSharp.Controllers
{

    [ApiController]
    [Route("api/agencias")]
    public class AgenciasController : ControllerBase
    {
        private readonly AppDbContext _ctx;

        public AgenciasController(AppDbContext ctx) => _ctx = ctx;

        // POST /api/agencias
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] Agencia agencia)
        {
            if (string.IsNullOrWhiteSpace(agencia.Nome))
                return BadRequest("Nome da agência é obrigatório.");

            _ctx.Agencias.Add(agencia);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarPorId), new { id = agencia.Id }, agencia);
        }

        // GET /api/agencias/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var agencia = await _ctx.Agencias.FindAsync(id);

            if (agencia is null)
                return NotFound("Agência não encontrada.");

            return Ok(agencia);
        }
    }
}
