using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuezSeguro.Models  
{
    public class Expediente
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de expediente es obligatorio")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Entre 5 y 30 caracteres")]
        [Display(Name = "Número de expediente")]
        [RegularExpression(@"^[A-Z0-9\-]+$", ErrorMessage = "Solo mayúsculas, números y guiones")]
        public string NumeroExpediente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Entre 5 y 200 caracteres")]
        [Display(Name = "Título del caso")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [StringLength(2000, MinimumLength = 10)]
        [Display(Name = "Descripción / Hechos")]
        [DataType(DataType.MultilineText)]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar una materia")]
        [Display(Name = "Materia")]
        public MateriaJuridica Materia { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public EstadoExpediente Estado { get; set; } = EstadoExpediente.Abierto;

        [Display(Name = "Fecha de apertura")]
        [DataType(DataType.DateTime)]
        public DateTime FechaApertura { get; set; } = DateTime.UtcNow;

        [Display(Name = "Fecha de cierre")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaCierre { get; set; }

        [StringLength(128)]
        [Display(Name = "Hash de integridad (SHA-512)")]
        public string? HashIntegridad { get; set; }

        [StringLength(2048)]
        [Display(Name = "Firma criptográfica")]
        public string? FirmaDigital { get; set; }

        [Required]
        [StringLength(450)] 
        [Display(Name = "Creado por")]
        public string CreadoPorId { get; set; } = string.Empty;

        [StringLength(450)]
        [Display(Name = "Última modificación por")]
        public string? ModificadoPorId { get; set; }

        [Display(Name = "Última modificación")]
        public DateTime? FechaModificacion { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; } = true; 
    }

    public enum MateriaJuridica
    {
        [Display(Name = "Civil")] Civil = 1,
        [Display(Name = "Penal")] Penal = 2,
        [Display(Name = "Laboral")] Laboral = 3,
        [Display(Name = "Mercantil")] Mercantil = 4,
        [Display(Name = "Familia")] Familia = 5,
        [Display(Name = "Tributario")] Tributario = 6,
        [Display(Name = "Constitucional")] Constitucional = 7,
        [Display(Name = "Administrativo")] Administrativo = 8
    }

    public enum EstadoExpediente
    {
        [Display(Name = "Abierto")] Abierto = 1,
        [Display(Name = "En proceso")] EnProceso = 2,
        [Display(Name = "En revisión")] EnRevision = 3,
        [Display(Name = "Cerrado")] Cerrado = 4,
        [Display(Name = "Archivado")] Archivado = 5
    }
}