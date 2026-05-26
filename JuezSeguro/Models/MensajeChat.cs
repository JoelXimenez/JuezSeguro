using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuezSeguro.Models
{
    /// <summary>
    /// Mapea la tabla TBL_MENSAJE_CHAT de BD_JuezSeguro.
    /// </summary>
    public class MensajeChat
    {
        // bigint IDENTITY en la BD → long en C#
        public long Id { get; set; }

        [Required]
        [StringLength(450)]
        public string UsuarioId { get; set; } = string.Empty;

        [Required]
        [StringLength(128)]
        public string PseudonimoEmisor { get; set; } = string.Empty;

        [Required(ErrorMessage = "El mensaje no puede estar vacío.")]
        [StringLength(1000, MinimumLength = 1)]
        public string Texto { get; set; } = string.Empty;

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public string Sala { get; set; } = "general";
    }
}
