using Data;
using Data.Models.Entidade;
using Repository.Produtos;

namespace Service.Services
{

    public class ProdutoService : IProdutoService
    {
        private IProdutosRepositorio _repository;
        public ProdutoService(ApiLojaVirtualContext context)
        {
            _repository = new RepositorioProdutos(context);
        }
        public Produto ProdutoUrl(string produtoUrl)
        {
            var produto = _repository.Produtos()
                .FirstOrDefault(x => x.Url == produtoUrl);

            return produto == null ? null! :
                produto.Ativo == true ? produto : null! ;
        }
    }

    public interface IProdutoService
    {
        Produto ProdutoUrl(string produtoUrl);
    }
}
