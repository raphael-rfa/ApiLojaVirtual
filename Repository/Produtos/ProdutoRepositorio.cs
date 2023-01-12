using Data;
using Data.Models.Entidade;

namespace Repository.Produtos
{
    public class RepositorioProdutos : IProdutosRepositorio
    {
        private readonly ApiLojaVirtualContext _context;

        public RepositorioProdutos(ApiLojaVirtualContext context)
        {
            _context = context;
        }        

        public ICollection<Produto> Produtos()
        {
            return _context.Produto.ToList();
        }
    }
}
