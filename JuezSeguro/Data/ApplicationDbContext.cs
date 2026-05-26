using JuezSeguro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JuezSeguro.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Expediente> Expedientes { get; set; }
        public DbSet<MensajeChat> MensajesChat { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Expediente>().ToTable("TBL_EXPEDIENTE", t => t.ExcludeFromMigrations());
            builder.Entity<MensajeChat>(e =>
            {
                e.ToTable("TBL_MENSAJE_CHAT", t => t.ExcludeFromMigrations());

                e.HasKey(m => m.Id);

                e.Property(m => m.Id)
                 .HasColumnName("ID_MENSAJE")
                 .ValueGeneratedOnAdd();

                e.Property(m => m.UsuarioId)
                 .HasColumnName("USER_ID_EMISOR")
                 .HasMaxLength(450)
                 .IsRequired();

                e.Property(m => m.PseudonimoEmisor)
                 .HasColumnName("PSEUDONIMO_EMISOR")
                 .HasMaxLength(128)
                 .IsRequired();

                e.Property(m => m.Texto)
                 .HasColumnName("TEXTO")
                 .HasMaxLength(1000)
                 .IsRequired();

                e.Property(m => m.Fecha)
                 .HasColumnName("FECHA_ENVIO")
                 .HasColumnType("datetime")
                 .HasDefaultValueSql("GETUTCDATE()");

                e.Property(m => m.Sala)
                 .HasColumnName("SALA")
                 .HasMaxLength(50)
                 .HasDefaultValue("general");

                // Índice que ya existe en BD: (SALA, FECHA_ENVIO DESC)
                e.HasIndex(m => new { m.Sala, m.Fecha })
                 .HasDatabaseName("IX_MENSAJE_CHAT_FECHA");
            });
        }
    }
}
