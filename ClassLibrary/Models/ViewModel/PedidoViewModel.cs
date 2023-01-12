

using Data.Models.Entidade;

namespace Data.Models.ViewModel
{
    public class PedidoViewModel
    {
        public Pedido? Pedido { get; set; }
        public ICollection<PedidoItem>? Itens { get; set; }

    }
}
