using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP2_CSharp.Models
{
    [Table("Clientes")]
    public abstract class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        public int AgenciaId { get; set; }

        [ForeignKey("AgenciaId")]
        public Agencia? Agencia { get; set; }
    }
}
