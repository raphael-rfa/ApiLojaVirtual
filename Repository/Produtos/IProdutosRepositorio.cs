using Data.Models;

namespace Repository.Produtos
{
    public interface IProdutosRepositorio
    {
        Produto ProdutoUrl(string produtoUrl);
    }
}
