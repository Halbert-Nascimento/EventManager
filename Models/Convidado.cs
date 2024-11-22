using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Models
{
    public class Convidado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Sex { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        // Chave estrangeira para Evento
        [ForeignKey("Evento")]
        public int EventoId { get; set; }

        public Evento? Evento { get; set; }





    }
}
