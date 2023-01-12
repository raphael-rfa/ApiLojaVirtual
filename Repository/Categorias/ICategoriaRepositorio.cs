using Data.Models.Entidade;

namespace Repository.Categorias
{
    public interface ICategoriaRepositorio
    {
        ICollection<Categoria> Categorias();
        IEnumerable<Produto> ProdutosValidos(int categoriaId);
    }
}
