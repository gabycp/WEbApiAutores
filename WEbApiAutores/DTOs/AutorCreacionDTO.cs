using System.ComponentModel.DataAnnotations;
using WEbApiAutores.Validaciones;

namespace WEbApiAutores.DTOs
{
    public class AutorCreacionDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(150, ErrorMessage = "El campo {0} no debe pasar de los {1} carácteres")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
    }
}
