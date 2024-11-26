using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventManager.Models
{
    public class Evento
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Título é obrigatório.")]
        [StringLength(100)]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Por favor adicione uma descrição.")]
        [StringLength(500)]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "Por favor adicione o local que ocorrera o evento.")]
        public string Local { get; set; } = string.Empty;

        [Required(ErrorMessage = "Por favor adicione a data do evento.")]
        public DateOnly Data {  get; set; }

        [Required(ErrorMessage = "Por favor adicione o horario do evento.")]
        public TimeOnly Hora { get; set; }

        [Required(ErrorMessage = "Por favor adicione quantas pessoas o evento suporta")]
        public int QuantidadeMaxPessoas { get; set; }

        [Required(ErrorMessage = "Por favor adicione o valor do ingresso, 0 para gratuito")]
        [Range(0, double.MaxValue)]
        public decimal valorIngresso { get; set; }


        // Relacionamento com Convidados
        public ICollection<Convidado>? Convidados { get; set; }

        // Chave estrangeira para Evento
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }
    }
}
