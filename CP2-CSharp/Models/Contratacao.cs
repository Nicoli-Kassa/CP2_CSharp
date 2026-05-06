using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP2_CSharp.Models
{
    [Table("Contratacoes")]
    public class Contratacao
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public decimal ValorSolicitado { get; set; }
        public string Status { get; set; } = "PENDENTE";
    }
}