using System.ComponentModel.DataAnnotations;
using WEbApiAutores.Validaciones;

namespace WEbApiAutores.DTOs
{
    public class LibroPatchDTO
    {
        [StringLength(maximumLength: 250)]
        [Required]
        public string Titulo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
