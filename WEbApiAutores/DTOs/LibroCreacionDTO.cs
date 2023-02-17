using System.ComponentModel.DataAnnotations;

namespace WEbApiAutores.DTOs
{
    public class LibroCreacionDTO
    {
        [StringLength(maximumLength: 250)]
        public string Titulo { get; set; }
        public List<int> AutoresId { get; set; }
    }
}
