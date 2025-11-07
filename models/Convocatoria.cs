using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiProyecto8;

public class Convocatoria
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ConvocatoriaId { get; set; }
    public string Nombre { get; set; }
    [Column(TypeName = "decimal(5,2)")]

    public decimal Costo { get; set; }
}
