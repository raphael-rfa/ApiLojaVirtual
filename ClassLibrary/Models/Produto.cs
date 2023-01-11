using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    public class Produto
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }
        public string? Nome { get; set; }
        public string? Url { get; set; }
        public int Quantidade { get; set; }
        public bool Ativo
        {
            get { return Ativo = Quantidade > 0; }
            set { }
        }
        public bool Excluido { get; set; }
    }
}
