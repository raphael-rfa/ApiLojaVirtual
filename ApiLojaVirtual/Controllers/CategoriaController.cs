using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Repository.Categorias;

namespace ApiLojaVirtual.Controllers
{
    [ApiController]
    [Route("Api/[Controller]/[Action]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaRepositorio _repositorio;

        public CategoriaController(ApiLojaVirtualContext context)
        {
            _repositorio = new CategoriaRepositorio(context);
        }

        [HttpGet]
        public IActionResult ListaCategorias()
        {
            var categorias = _repositorio.ListarCategorias();

            return categorias != null ? Ok(categorias) : Problem("Sem categorias validas");
        }

        [HttpGet]
        public IActionResult CategoriaUrl(string categoriaUrl)
        {
            var categoria = _repositorio.CategoriaUrl(categoriaUrl);

            return categoria != null ? Ok(categoria) : Problem("Categoria invalida");
        }
    }
}
