
namespace Application.Models.Churrasco
{
    public class DetalhesChurrasco
    {
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public int Total_Participantes { get; set; }    
        public Decimal Valor_Total_Arrecadado { get; set; }
        public List<Participantes>? Lista_Participantes { get; set; }

        public class Participantes
        {
            public int Id { get; set; } 
            public string? Nome { get; set; }
            public Decimal ValorPago { get; set; }
        }
    }
}
