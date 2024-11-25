using EventManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Data
{
    public class GerenciadorEventosContext : DbContext
    {
        public GerenciadorEventosContext(DbContextOptions<GerenciadorEventosContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Convidado> Convidados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração do relacionamento 1:N entre EveUsuario e Eventos
            modelBuilder.Entity<Evento>()
                .HasOne(c => c.Usuario)
                .WithMany(e => e.Eventos)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        

        // Configuração do relacionamento 1:N entre Evento e Convidados
        modelBuilder.Entity<Convidado>()
                .HasOne(c => c.Evento)
                .WithMany(e => e.Convidados)
                .HasForeignKey(c => c.EventoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        
    }
}
