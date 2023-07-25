using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using WEbApiAutores.Validaciones;

namespace WebAPIAutores.Tests.PruebasUnitarias
{
    [TestClass]
    public class PrimeraLetraMayusculaAttributeTests
    {
        [TestMethod]
        public void PrimeraLetraMinuscula_DevuelveError()
        {
            //Preparacion
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            var valor = "gabriela";
            var valorContext = new ValidationContext(new { Nombre = valor });

            //Prueba
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valorContext);

            //Verificacion
            Assert.AreEqual("La primera letra debe de ser mayúscula", resultado.ErrorMessage);
        }

        [TestMethod]
        public void ValorNulo_NoDevuelveError()
        {
            //Preparacion
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            string valor = null;
            var valorContext = new ValidationContext(new { Nombre = valor });

            //Prueba
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valorContext);

            //Verificacion
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void PrimeraLetraMayuscula_NoDevuelveError()
        {
            //Preparacion
            var primeraLetraMayuscula = new PrimeraLetraMayusculaAttribute();
            string valor = "Gabriela";
            var valorContext = new ValidationContext(new { Nombre = valor });

            //Prueba
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valorContext);

            //Verificacion
            Assert.IsNull(resultado);
        }
    }
}