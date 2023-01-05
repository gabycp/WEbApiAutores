using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WEbApiAutores.Validaciones;

namespace WEbApiAutores.Controllers.Entidades
{
    public class Autor: IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo {0} es requerido.")]
        [StringLength(150,ErrorMessage ="El campo {0} no debe pasar de los {1} carácteres")]
        //[PrimeraLetraMayuscula]
        public String Nombre { get; set; }
        [Range(18,102)]
        [NotMapped]
        public int Edad { get; set; }
        [CreditCard]
        [NotMapped]
        public string TarjetaCredito { get; set; }
        [Url]
        [NotMapped]
        public string Url { get; set; }
        [NotMapped]
        public int Mayor { get; set; }
        [NotMapped]
        public int Menor { get; set; }
        public List<Libros> Libros { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Nombre))
            {
                var primeraLetra = Nombre[0].ToString();

                if(primeraLetra != primeraLetra.ToUpper())
                {
                    yield return new ValidationResult("La primera letra debe de ser mayuscula",
                                                            new string[] {nameof(Nombre)});
                }
            }

            if (Menor > Mayor)
            {
                yield return new ValidationResult("El número menor es mayor al Numero Mayor",
                                                    new string[] { nameof(Menor) });
            }
        }
    }
}
