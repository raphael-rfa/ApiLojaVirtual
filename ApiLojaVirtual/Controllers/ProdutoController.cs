using ApiLojaVirtual.Data.Context;
using ApiLojaVirtual.Data.Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApiLojaVirtual.Controllers
{
    [ApiController]
    [Route("Api/[Controller]/[Action]")]
    public class ProdutoController : ControllerBase
    {
        private readonly ApiLojaVirtualContext _context;

        public ProdutoController(ApiLojaVirtualContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ProdutoUrl(string produtoUrl)
        {
            var produto = _context.Produto.FirstOrDefault(x => x.Url == produtoUrl);

            if (produto != null)
            {
                if (produto.Ativo == true)
                {
                    return Ok(produto);
                }

                return Problem("Produto Inativo");
            }
            return Problem("Produto não encontrado");
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

                if(produto != null)
                {
                    produto!.Quantidade -= item.Quantidade;

                    produto.Ativo = false ? produto.Quantidade == 0 : produto.Ativo = true;

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
        public IActionResult ListaPedidos()
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
