using EventManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManager.Data
{
    public class GerenciadorEventosContext : DbContext
    {
        public GerenciadorEventosContext(DbContextOptions<GerenciadorEventosContext> options) : base(options) { }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Convidado> Convidados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração do relacionamento 1:N entre Evento e Convidados
            modelBuilder.Entity<Convidado>()
                .HasOne(c => c.Evento)
                .WithMany(e => e.Convidados)
                .HasForeignKey(c => c.EventoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        public DbSet<EventManager.Models.Usuario> Usuario { get; set; } = default!;
    }
}
