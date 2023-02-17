using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEbApiAutores.Validaciones;

namespace WEbApiAutores.Entidades
{
    public class Autor 
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(150, ErrorMessage = "El campo {0} no debe pasar de los {1} carácteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        public List<Libros> Libros { get; set; }
        public List<AutoresLibros> AutoresLibros { get; set;}

    }
}
