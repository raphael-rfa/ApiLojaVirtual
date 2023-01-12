using Data;
using Data.Models.Entidade;
using Data.Models.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Repository.Pedidos
{
    public class PedidoRepositorio : IPedidosRepositorio
    {
        private readonly ApiLojaVirtualContext _context;

        public PedidoRepositorio(ApiLojaVirtualContext context)
        {
            _context = context;
        }

        public IEnumerable<Pedido> Pedidos(int userId)
        {
                var pedidos = _context.Pedido.Where(x => x.UsuarioId == userId);
            return pedidos;
        }

        public IEnumerable<PedidoViewModel> ListarPedidos(int userId)
        {
            var pedidos = Pedidos(userId);

            List<PedidoViewModel>? listaPedidos = new();

            if(pedidos != null)
            {
                foreach (var p in pedidos)
                {

                    PedidoViewModel pedido = new()
                    {
                        Pedido = p,
                        Itens = _context.PedidoItem.Where(x => x.PedidoId == p.Id).ToList()
                    };

                    listaPedidos.Add(pedido);
                }
            }

            return listaPedidos.ToList();
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
