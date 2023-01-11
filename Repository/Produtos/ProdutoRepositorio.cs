using Data;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Produtos
{
    public class RepositorioProdutos : IProdutosRepositorio
    {
        private readonly ApiLojaVirtualContext _context;

        public RepositorioProdutos(ApiLojaVirtualContext context)
        {
            _context = context;
        }        

        public Produto ProdutoUrl(string produtoUrl)
        {
            return _context.Produto.FirstOrDefault(x => x.Url == produtoUrl)!;
        }
    }
}
