using System.ComponentModel.DataAnnotations;

namespace EventManager.Models
{
    public class Evento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        public string Local { get; set; } = string.Empty;

        [Required]
        public DateOnly Data {  get; set; }

        [Required]
        public TimeOnly Hora { get; set; }

        public int QuantidadeMaxPessoas { get; set; }

        [Range(0, double.MaxValue)]
        public decimal valorIngresso { get; set; }

        // Relacionamento com Convidados
        public ICollection<Convidado>? Convidados { get; set; }

    }
}
