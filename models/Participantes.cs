using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiProyecto8
{
    public class Participantes
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress]
        [StringLength(150)]
        public string Correo { get; set; }
        public int IdConvocatoria { get; set; } 
        [ForeignKey("IdConvocatoria")]
        public virtual Convocatoria Convocatoria { get; set; }
    }
}
