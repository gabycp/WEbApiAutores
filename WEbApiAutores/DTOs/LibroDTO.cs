using System.ComponentModel.DataAnnotations;

namespace WEbApiAutores.DTOs
{
    public class LibroDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }

        //public List<ComentarioDT> Comentarios { get; set; }

    }
}
