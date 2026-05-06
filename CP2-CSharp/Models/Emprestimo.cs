using System.ComponentModel.DataAnnotations.Schema;

namespace CP2_CSharp.Models
{
    [Table("Emprestimos")]
    public class Emprestimo : Produto
    {
        public decimal ValorMaximo { get; set; }
        public decimal TaxaJuros { get; set; }
        public int PrazoMeses { get; set; }

        // Regra de negócio real (exigido para dupla)
        public string Avaliar(decimal valorSolicitado)
        {
            if (valorSolicitado > ValorMaximo)
                return "REPROVADO: valor acima do limite permitido.";

            decimal totalComJuros = valorSolicitado * (1 + TaxaJuros / 100 * PrazoMeses / 12);

            if (TaxaJuros > 10)
                return $"APROVADO COM RESSALVA: taxa alta ({TaxaJuros}%). Total a pagar: {totalComJuros:C}";

            return $"APROVADO. Total a pagar em {PrazoMeses}x: {totalComJuros:C}";
        }
    }
}