using Data;
using Data.Models.Entidade;
using Repository.Categorias;

namespace Service.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepositorio _repository;
        public CategoriaService(ApiLojaVirtualContext context)
        {
            _repository = new CategoriaRepositorio(context);
        }

        public Categoria CategoriaValida(Categoria categoria)
        {
            categoria.Produtos = _repository.ProdutosValidos(categoria.Id).ToList();
            categoria.Ativo = categoria.Produtos.Any();

            return categoria.Ativo == true ? categoria : null!;
        }

        public Categoria CategoriaUrl(string categoriaUrl)
        {
            var categoria = _repository.Categorias().FirstOrDefault(x => x.Url == categoriaUrl);

            return categoria == null ? null! :
                CategoriaValida(categoria) != null ? categoria : null!;
        }

        public ICollection<Categoria> ListarCategorias()
        {
            var categorias = _repository.Categorias().ToList();

            var categoriasValidas = new List<Categoria>();

            foreach (var categoria in categorias)
            {                
                categoriasValidas.Add(CategoriaValida(categoria));
            }

            return categoriasValidas.ToList();            
        }
    }

    public interface ICategoriaService
    {
        Categoria CategoriaUrl(string categoriaUrl);
        ICollection<Categoria> ListarCategorias();
    }
}
