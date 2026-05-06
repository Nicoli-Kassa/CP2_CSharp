using CP2_CSharp.Data;
using CP2_CSharp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CP2_CSharp.Controllers
{
    [ApiController]
    [Route("api/emprestimos")]
    public class EmprestimosController : ControllerBase
    {
        private readonly AppDbContext _ctx;
        public EmprestimosController(AppDbContext ctx) => _ctx = ctx;

        // POST /api/emprestimos
        [HttpPost]
        public async Task<IActionResult> Cadastrar([FromBody] Emprestimo emprestimo)
        {
            if (emprestimo.ValorMaximo <= 0)
                return BadRequest("Valor máximo deve ser maior que zero.");

            if (emprestimo.TaxaJuros <= 0 || emprestimo.TaxaJuros > 100)
                return BadRequest("Taxa de juros inválida.");

            if (emprestimo.PrazoMeses <= 0)
                return BadRequest("Prazo em meses deve ser maior que zero.");

            _ctx.Emprestimos.Add(emprestimo);
            await _ctx.SaveChangesAsync();

            return CreatedAtAction(nameof(BuscarPorId), new { id = emprestimo.Id }, emprestimo);
        }

        // GET /api/emprestimos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var emprestimo = await _ctx.Emprestimos.FindAsync(id);
            if (emprestimo is null)
                return NotFound("Empréstimo não encontrado.");
            return Ok(emprestimo);
        }
    }
}