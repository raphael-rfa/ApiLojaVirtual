using Data;
using Data.Models.Entidade;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Pedidos;
using Repository.Produtos;

namespace ApiLojaVirtual.Controllers
{
    [ApiController]
    [Route("Api/[Controller]/[Action]")]
    public class PedidoController : Controller
    {
        private readonly PedidoRepositorio _repository;

        public PedidoController(ApiLojaVirtualContext context)
        {
            _repository = new PedidoRepositorio(context);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CriarPedido(ICollection<PedidoItem> itens, int userId)
        {
            var pedido = _repository.CriarPedido(itens, userId);
            return pedido ? Ok("Pedido Criado") : Problem("Pedido não foi criado");
        }

        [Authorize]
        [HttpGet]
        public IActionResult ListarPedidos(int userId)
        {
            var Pedidos = _repository.ListarPedidos(userId);

            return Pedidos != null ? Ok(Pedidos) : Problem("Pedidos não encontrados");
        }
    }
}
