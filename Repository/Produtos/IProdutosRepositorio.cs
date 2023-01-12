using Data.Models.Entidade;

namespace Repository.Produtos
{
    public interface IProdutosRepositorio
    {
        Produto ProdutoUrl(string produtoUrl);
    }
}
