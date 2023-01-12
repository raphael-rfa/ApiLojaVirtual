using Data;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace ApiLojaVirtual.Controllers
{
    [ApiController]
    [Route("Api/[Controller]/[Action]")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _service;

        public CategoriaController(ApiLojaVirtualContext context)
        {
            _service = new CategoriaService(context);
        }

        [HttpGet]
        public IActionResult ListaCategorias()
        {
            var categorias = _service.ListarCategorias();

            return categorias != null ? Ok(categorias) : Problem("Sem categorias validas");
        }

        [HttpGet]
        public IActionResult CategoriaUrl(string categoriaUrl)
        {
            var categoria = _service.CategoriaUrl(categoriaUrl);

            return categoria != null ? Ok(categoria) : Problem("Categoria invalida");
        }
    }
}
