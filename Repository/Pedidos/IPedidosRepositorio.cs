using Data.Models.Entidade;
using Data.Models.ViewModel;

namespace Repository.Pedidos
{
    internal interface IPedidosRepositorio
    {
        IEnumerable<PedidoViewModel> ListarPedidos(int userId);
        IEnumerable<Pedido> Pedidos(int userId);
        bool CriarPedido(ICollection<PedidoItem> itens, int userId);
    }
}
