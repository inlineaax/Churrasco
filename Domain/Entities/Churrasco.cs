using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    [Table("churrasco")]
    public class Churrasco
    {
        [Key, Column("id_churrasco")]
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "mm/dd/yyyy")]
        [Column("data")]
        public DateTime Data { get; set; }

        [Column("descricao")]
        public string? Descricao { get; set; }

        [Column("valor_sugerido_com_bebida")]
        public decimal Valor_Sugerido_Com_Bebida { get; set; }

        [Column("valor_sugerido_sem_bebida")]
        public decimal Valor_Sugerido_Sem_Bebida { get; set; }
        public List<Participante>? Participantes { get; set; }
    }
}
