using System.ComponentModel.DataAnnotations;

namespace EventManager.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string Nome { get; set; }

        [Required]
        [StringLength(500)]
        public required string Email { get; set; }

        [Required]
        public required string SenhaHash { get; set; }

        // Relacionamento com eventos
        public ICollection<Evento>? Eventos { get; set; }
    }
}
