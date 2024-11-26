using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EventManager.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Por favor adicione o nome.")]
        [StringLength(100)]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "Por favor adicione Email.")]
        [StringLength(500)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Por favor adicione a senha.")]
        public required string SenhaHash { get; set; }

        // Relacionamento com eventos
        public ICollection<Evento>? Eventos { get; set; }

        //função para comparar a senha do usuário ao hash armazenado
        public bool VerificarSenha(string senha)
        {
            return BCrypt.Net.BCrypt.Verify(senha, SenhaHash);
        }
    }
}