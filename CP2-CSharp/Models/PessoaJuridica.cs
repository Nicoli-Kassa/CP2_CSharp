using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP2_CSharp.Models
{
    [Table("PessoasJuridicas")]
    public class PessoaJuridica : Cliente
    {
        [Required]
        public string Cnpj { get; set; } = string.Empty;

        public string RazaoSocial { get; set; } = string.Empty;
    }
}
