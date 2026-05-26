using JuezSeguro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JuezSeguro.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
    {
        public DbSet<Expediente> Expedientes { get; set; }
        public DbSet<MensajeChat> MensajesChat { get; set; }
    }
}
