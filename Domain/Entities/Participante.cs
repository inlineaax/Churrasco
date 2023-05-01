using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("participante")]
    public class Participante
    {
        [Key, Column("id_participante")]
        public int Id { get; set; }

        [Column("id_churrasco")]
        public int Id_Churrasco { get; set; }

        [Column("Nome")]
        public string? Nome { get; set; }

        [Column("valor_pago")]
        public Decimal Valor_Pago { get; set; }

        public Churrasco? Churrasco { get; set; }
    }
}
