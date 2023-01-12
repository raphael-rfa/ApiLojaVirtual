using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models.Entidade
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Usuario")]
        public int UsuarioId { get; set; }
        public DateTime DataPedido { get; set; }
        public ICollection<PedidoItem>? PedidoItems { get; set; }
    }
}
