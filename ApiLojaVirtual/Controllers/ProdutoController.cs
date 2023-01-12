using Data;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace ApiLojaVirtual.Controllers
{
    [ApiController]
    [Route("Api/[Controller]/[Action]")]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoService _service;
        
        public ProdutoController(ApiLojaVirtualContext context)
        {
            _service = new ProdutoService(context);
        }

        [HttpGet]
        public IActionResult ProdutoUrl(string produtoUrl)
        {
            var produto = _service.ProdutoUrl(produtoUrl);

            return produto != null ? Ok(produto) : Problem("Produto não encontrado");
        }        
    }
}