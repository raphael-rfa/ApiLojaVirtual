using ApiLojaVirtual.Data.Context;
using ApiLojaVirtual.Data.Entidades;
using Microsoft.AspNetCore.Mvc;

namespace ApiLojaVirtual.Controllers
{
    [ApiController]
    [Route("Api/[Controller]/[Action]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ApiLojaVirtualContext _context;

        public CategoriaController(ApiLojaVirtualContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult ListaCategorias()
        {
            var produtos = _context.Produto
                .Where(x => x.Quantidade > 0 && x.Ativo == true);


            if (produtos != null)
            {
                var listaCategorias = new List<Categoria>();

                foreach (var c in produtos)
                {
                    listaCategorias.Add(_context.Categoria.Find(c.CategoriaId)!);
                }

                listaCategorias = listaCategorias.Distinct().ToList();

                return Ok(listaCategorias);
            }

            return Problem("Nenhum categoria corresponde ao filtro");
        }

        [HttpGet]
        public IActionResult CategoriaUrl(string categoriaUrl)
        {
            var categoria = _context.Categoria.FirstOrDefault(x => x.Url == categoriaUrl);

            if (categoria != null)
            {
                if (categoria.Ativo == true)
                {
                    var produtos = _context.Produto.Where(x => x.CategoriaId == categoria.Id);

                    return Ok(produtos);
                }

                return Problem("Categoria esta inativa");
            }

            return Problem("Categoria não encontrada");

        }
    }
}
