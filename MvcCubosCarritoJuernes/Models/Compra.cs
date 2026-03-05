using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCubosCarritoJuernes.Models
{
    [Table("compra")]
    public class Compra
    {
        [Key]
        [Column("id_compra")]
        public int Id_compra { get; set; }
        [Column("id_cubo")]
        public int Id_cubo { get; set; }
        [Column("cantidad")]
        public int Cantidad { get; set; }
        [Column("precio")]
        public int Precio { get; set; }
        [Column("fechapedido")]
        public DateTime Fechapedido { get; set; }

    }
    
}
