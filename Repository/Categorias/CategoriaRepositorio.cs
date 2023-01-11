using Data;
using Data.Models;

namespace Repository.Categorias
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly ApiLojaVirtualContext _context;

        public CategoriaRepositorio(ApiLojaVirtualContext context)
        {
            _context = context;
        }

        public Categoria CategoriaUrl(string categoriaUrl)
        {

            var categoria = _context.Categoria.FirstOrDefault(x => x.Url == categoriaUrl && x.Ativo == true)!;
            if(categoria != null)
            {
                categoria.Produtos = ProdutosValidos(categoria.Id).ToList();
                return categoria;
            }
            return categoria!;
        }

        public IEnumerable<Produto> ProdutosValidos(int categoriaId)
        {
            return _context.Produto.Where(x => x.CategoriaId == categoriaId && x.Ativo == true);
        }

        public IEnumerable<Categoria> ListarCategorias()
        {
            var categoria = _context.Categoria
                .Where(x => x.Ativo == true).ToList();

            foreach (var c in categoria)
            {
                 c.Produtos = ProdutosValidos(c.Id).ToList();
            }
            return categoria;
        }
    }
}
