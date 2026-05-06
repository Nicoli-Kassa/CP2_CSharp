using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CP2_CSharp.Data;
using CP2_CSharp.Models;

namespace CP2_CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratacoesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IServiceScopeFactory _scopeFactory;

        public ContratacoesController(AppDbContext context, IServiceScopeFactory scopeFactory)
        {
            _context = context;
            _scopeFactory = scopeFactory;
        }

        // POST: api/Contratacoes
        [HttpPost]
        public async Task<IActionResult> SolicitarContratacao(Contratacao contratacao)
        {
            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == contratacao.ClienteId);

            if (cliente == null)
                return NotFound("Cliente não encontrado.");

            contratacao.Status = "PENDENTE";

            try
            {
                _context.Contratacoes.Add(contratacao);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, "Erro ao salvar contratação.");
            }
             
            _ = Task.Run(async () =>
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                await Task.Delay(3000); // simula fila

                var c = await context.Contratacoes.FindAsync(contratacao.Id);
                if (c != null)
                { 
                    c.Status = c.ValorSolicitado > 5000 ? "REPROVADO" : "APROVADO";
                    await context.SaveChangesAsync();
                }
            });

            return Accepted(new
            {
                mensagem = "Contratação recebida e em processamento",
                contratacao.Id,
                contratacao.Status
            });
        }

        // GET: api/Contratacoes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Contratacao>> GetStatus(int id)
        {
            var contratacao = await _context.Contratacoes
                .FirstOrDefaultAsync(c => c.Id == id);

            if (contratacao == null)
                return NotFound("Contratação não encontrada.");

            return Ok(contratacao);
        }

        // GET: api/Contratacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contratacao>>> GetAll()
        {
            return await _context.Contratacoes.ToListAsync();
        }
    }
}