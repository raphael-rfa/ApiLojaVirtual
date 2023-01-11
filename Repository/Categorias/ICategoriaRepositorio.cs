using Data.Models;

namespace Repository.Categorias
{
    public interface ICategoriaRepositorio
    {
        IEnumerable<Categoria> ListarCategorias();
        IEnumerable<Produto> ProdutosValidos(int categoriaId);
        Categoria CategoriaUrl(string categoriaUrl);
    }
}
