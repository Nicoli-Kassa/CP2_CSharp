using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CP2_CSharp.Models
{
    [Table("PessoasFisicas")]
    public class PessoaFisica : Cliente
    {
        [Required]
        public string Cpf { get; set; } = string.Empty;

        public DateTime DataNascimento { get; set; }
    }
}
