using Data.Models.Entidade;

namespace Repository.Produtos
{
    public interface IProdutosRepositorio
    {
        ICollection<Produto> Produtos();
    }
}
