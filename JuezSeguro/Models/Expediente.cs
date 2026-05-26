using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JuezSeguro.Models  
{
    [Table("TBL_EXPEDIENTE")]
    public class Expediente
    {
        [Column("ID_EXPEDIENTE")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de expediente es obligatorio")]
        [StringLength(32, MinimumLength = 5, ErrorMessage = "Entre 5 y 32 caracteres")]
        [Display(Name = "Número de expediente")]
        [Column("NUMERO_EXPEDIENTE")]
        [RegularExpression(@"^[A-Z0-9\-]+$", ErrorMessage = "Solo mayúsculas, números y guiones")]
        public string NumeroExpediente { get; set; } = string.Empty;

        [Required(ErrorMessage = "El título es obligatorio")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Entre 5 y 200 caracteres")]
        [Display(Name = "Título del caso")]
        [Column("TITULO")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(2000)]
        [Display(Name = "Descripción / Hechos")]
        [Column("DESCRIPCION")]
        [DataType(DataType.MultilineText)]
        public string? Descripcion { get; set; }

        // Mapeo modificado para ajustarse a TIPO_CASO_COD (varchar 16)
        [Required(ErrorMessage = "Debe escribir la materia/tipo")]
        [StringLength(16)]
        [Display(Name = "Materia")]
        [Column("TIPO_CASO_COD")]
        public string MateriaCod { get; set; } = string.Empty;

        // Mapeo modificado para ajustarse a ESTADO_COD (varchar 16)
        [Required(ErrorMessage = "Estado es obligatorio")]
        [StringLength(16)]
        [Display(Name = "Estado")]
        [Column("ESTADO_COD")]
        public string EstadoCod { get; set; } = "BORRADOR";

        [Required(ErrorMessage = "El ID del Tribunal es obligatorio")]
        [Display(Name = "Tribunal")]
        [Column("ID_TRIBUNAL")]
        public int IdTribunal { get; set; }

        [Display(Name = "Fecha de apertura")]
        [DataType(DataType.DateTime)]
        [Column("FECHA_APERTURA")]
        public DateTime FechaApertura { get; set; } = DateTime.UtcNow;

        [Display(Name = "Fecha de modificación")]
        [DataType(DataType.DateTime)]
        [Column("FECHA_MODIFICACION")]
        public DateTime? FechaModificacion { get; set; }

        [StringLength(128)]
        [Display(Name = "Hash de contenido (SHA-512)")]
        [Column("HASH_CONTENIDO")]
        public string? HashIntegridad { get; set; }

        [StringLength(512)]
        [Display(Name = "Firma criptográfica")]
        [Column("FIRMA_DIGITAL")]
        public string? FirmaDigital { get; set; }

        [Required]
        [StringLength(450)] 
        [Display(Name = "Creado por (User ID)")]
        [Column("USER_ID_CREADOR")]
        public string CreadoPorId { get; set; } = string.Empty;

        [Required]
        [StringLength(128)]
        [Display(Name = "Pseudónimo")]
        [Column("PSEUDONIMO_CREADOR")]
        public string PseudonimoCreador { get; set; } = string.Empty;
    }
}