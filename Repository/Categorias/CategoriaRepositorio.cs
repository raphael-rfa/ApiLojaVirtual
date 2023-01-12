using Data;
using Data.Models.Entidade;
using Microsoft.EntityFrameworkCore;

namespace Repository.Categorias
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private readonly ApiLojaVirtualContext _context;

        public CategoriaRepositorio(ApiLojaVirtualContext context)
        {
            _context = context;
        }

        public IEnumerable<Produto> ProdutosValidos(int categoriaId)
        {
            return _context.Produto.Where(x => x.CategoriaId == categoriaId && x.Ativo == true) ?? null!;
        }

        public ICollection<Categoria> Categorias()
        {
            return _context.Categoria
                .Include(x => x.Produtos).ToList();
        }
    }
}
