using System.ComponentModel.DataAnnotations;

namespace ApiLojaVirtual.Data.Entidades
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Url { get; set; }
        public bool Ativo { get; set; }
        public bool Excluido { get; set; }
    }
}
