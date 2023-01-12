using Data;
using Data.Models.Entidade;
using Microsoft.EntityFrameworkCore;

namespace Repository.Pedidos
{
    public class PedidoRepositorio : IPedidosRepositorio
    {
        private readonly ApiLojaVirtualContext _context;

        public PedidoRepositorio(ApiLojaVirtualContext context)
        {
            _context = context;
        }

        public IEnumerable<Pedido> ListarPedidos(int userId)
        {
            var pedidos = _context.Pedido
                .Include(x => x.PedidoItems)
                .Where(x => x.UsuarioId == userId);

            return pedidos!;
        }

        public bool CriarPedido(ICollection<PedidoItem> itens, int userId)
        {
            Pedido pedido = new();
            pedido.DataPedido = DateTime.Now;
            pedido.UsuarioId = userId;

            _context.Pedido.Add(pedido);
            _context.SaveChanges();                                        

            
            foreach (var item in itens)
            {
                var produto = _context.Produto.Find(item.ProdutoId)!;

                if (item.Quantidade <= produto.Quantidade)
                {
                    produto.Quantidade -= item.Quantidade;

                    item.PedidoId = pedido.Id;

                    _context.PedidoItem.Add(item);

                    _context.SaveChanges();                                        
                }
                else
                {
                    _context.Pedido.Remove(pedido);
                    return false;
                }
            }

            return true;
        }
    }
}
