using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiLojaVirtual.Controllers
{
    [ApiController]
    [Route("Api/[Controller]/[Action]")]
    public class PedidoController : Controller
    {
        private readonly ApiLojaVirtualContext _context;

        public PedidoController(ApiLojaVirtualContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public IActionResult CriarPedido(ICollection<PedidoItem> itens, int userId)
        {
            foreach (var item in itens)
            {
                Produto? produto = _context.Produto
                    .Where(x => x.Ativo == true && x.Quantidade >= item.Quantidade)
                    .FirstOrDefault(x => x.Id == item.ProdutoId);

                if (produto != null)
                {
                    produto!.Quantidade -= item.Quantidade;

                    Pedido pedido = new();
                    pedido.DataPedido = DateTime.Now;
                    pedido.UsuarioId = userId;
                    _context.Pedido.Add(pedido);
                    _context.SaveChanges();

                    item.PedidoId = pedido.Id;

                    _context.PedidoItem.Add(item);
                    _context.SaveChanges();

                    return Ok("Pedido feito");
                }
                return Problem("Produto invalido");
            }
            return Problem("Lista Vazia");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ListarPedidos()
        {
            var Pedidos = from pedidos in _context.Pedido
                          join item in _context.PedidoItem on pedidos.Id equals item.PedidoId into p
                          select new
                          {
                              IdPedido = pedidos.Id,
                              Itens = p.Count()
                          };

            return Ok(Pedidos);
        }
    }
}
