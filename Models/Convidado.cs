using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Models
{
    public class Convidado
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor adicione o nome")]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Por favor selecione o sexo")]
        [StringLength(50)]
        public string Sex { get; set; } = string.Empty;

        [Required(ErrorMessage = "Por favor adicione email para contato")]
        public string Email { get; set; } = string.Empty;

        // Chave estrangeira para Evento
        [ForeignKey("Evento")]
        public int EventoId { get; set; }

        public Evento? Evento { get; set; }





    }
}
