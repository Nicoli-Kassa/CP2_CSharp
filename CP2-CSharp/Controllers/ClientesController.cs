using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CP2_CSharp.Data;
using CP2_CSharp.Models;

namespace CP2_CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ClientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        // POST: api/Clientes/pf
        [HttpPost("pf")]
        public async Task<ActionResult<PessoaFisica>> CadastrarPF(PessoaFisica pf)
        {
            var agencia = await _context.Agencias
                .FirstOrDefaultAsync(a => a.Id == pf.AgenciaId);

            if (agencia == null)
                return NotFound("Agência não encontrada.");

            var cpfExistente = await _context.PessoasFisicas
                .FirstOrDefaultAsync(x => x.Cpf == pf.Cpf);

            if (cpfExistente != null)
                return BadRequest("CPF já cadastrado."); 

            try
            {
                _context.PessoasFisicas.Add(pf);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            { 
                return BadRequest("CPF já cadastrado (constraint do banco).");
            }

            return Created("", pf);
        }

        // POST: api/Clientes/pj
        [HttpPost("pj")]
        public async Task<ActionResult<PessoaJuridica>> CadastrarPJ(PessoaJuridica pj)
        {
            var agencia = await _context.Agencias
                .FirstOrDefaultAsync(a => a.Id == pj.AgenciaId);

            if (agencia == null)
                return NotFound("Agência não encontrada.");

            var cnpjExistente = await _context.PessoasJuridicas
              .FirstOrDefaultAsync(x => x.Cnpj == pj.Cnpj);

            if (cnpjExistente != null)
                return BadRequest("CNPJ já cadastrado.");

            try
            {
                _context.PessoasJuridicas.Add(pj);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest("CNPJ já cadastrado (constraint do banco).");
            }

            return Created("", pj);
        }
    }
}