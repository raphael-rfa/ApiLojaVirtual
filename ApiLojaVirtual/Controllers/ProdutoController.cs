using Data;
using Microsoft.AspNetCore.Mvc;
using Repository.Produtos;

namespace ApiLojaVirtual.Controllers
{
    [ApiController]
    [Route("Api/[Controller]/[Action]")]
    public class ProdutoController : ControllerBase
    {
        private IProdutosRepositorio _repository;
        public ProdutoController(ApiLojaVirtualContext context)
        {
            this._repository = new RepositorioProdutos(context);
        }

        [HttpGet]
        public IActionResult ProdutoUrl(string produtoUrl)
        {
            var produto = _repository.ProdutoUrl(produtoUrl);

            if (produto != null)
            {
                if (produto.Ativo == true)
                {
                    return Ok(produto);
                }

                return Problem("Sem Estoque");
            }
            return Problem("Produto não encontrado");
        }        
    }
}
