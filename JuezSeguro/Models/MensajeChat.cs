using System.ComponentModel.DataAnnotations;

namespace JuezSeguro.Models  
{
    public class MensajeChat
    {
        public int Id { get; set; }

        [Required]
        [StringLength(450)]
        public string UsuarioId { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        [Display(Name = "Usuario")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "El mensaje no puede estar vacío")]
        [StringLength(1000, MinimumLength = 1)]
        [Display(Name = "Mensaje")]
        public string Texto { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de envío")]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Display(Name = "Expediente relacionado")]
        public int? ExpedienteId { get; set; }
    }
}