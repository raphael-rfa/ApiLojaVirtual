using ApiLojaVirtual.Data.Entidades;
using Microsoft.EntityFrameworkCore;

namespace ApiLojaVirtual.Data.Context
{
    public class ApiLojaVirtualContext : DbContext
    {
        public ApiLojaVirtualContext(DbContextOptions options) :base(options) { }

        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Produto> Produto { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Pedido> Pedido { get; set; }
        public DbSet<PedidoItem> PedidoItem { get; set; }
    }
}
