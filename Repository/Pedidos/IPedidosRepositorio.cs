using Data.Models.Entidade;

namespace Repository.Pedidos
{
    internal interface IPedidosRepositorio
    {
        IEnumerable<Pedido> ListarPedidos(int userId);
        bool CriarPedido(ICollection<PedidoItem> itens, int userId);
    }
}
