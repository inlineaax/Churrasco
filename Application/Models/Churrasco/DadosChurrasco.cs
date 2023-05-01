using System.ComponentModel.DataAnnotations;

namespace Application.Models.Churrasco
{
    public class DadosChurrasco
    {
        public DateTime Data { get; set; }
        public string? Descricao { get; set; }
        public Decimal ValorComBebida { get; set; }
        public Decimal ValorSemBebida { get; set; }
    }
}
