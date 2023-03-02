using System.ComponentModel.DataAnnotations;

namespace WEbApiAutores.Entidades
{
    public class Libros
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength:250)]
        public string Titulo { get; set; }
        public DateTime? FechaPublicacion { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public List<AutoresLibros> AutoresLibros { get; set; }

    }
}
