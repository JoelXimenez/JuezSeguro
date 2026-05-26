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
            builder.Entity<MensajeChat>().ToTable("TBL_MENSAJE_CHAT", t => t.ExcludeFromMigrations());
        }
    }
}
