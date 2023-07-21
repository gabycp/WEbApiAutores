using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Identity.Client;
using WEbApiAutores.DTOs;
using WEbApiAutores.Servicios;

namespace WEbApiAutores.Utilidades
{
    public class HATEOASAutorFilterAttribute: HATEOASFiltroAttribute
    {
        private readonly GeneradorEnlaces generadorEnlaces;

        public HATEOASAutorFilterAttribute( GeneradorEnlaces generadorEnlaces ) 
        {
            this.generadorEnlaces = generadorEnlaces;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            var debeIncluir = DebeIncluirHATEOAS(context);

            if (!debeIncluir) 
            {
                await next();
                return;
            }

            var resultado = context.Result as ObjectResult;

            //var modelo = resultado.Value as AutorDTO ??
            //throw new ArgumentNullException("Se esperaba una instancia de AutorDTO");

            var autorDTO = resultado.Value as AutorDTO;
            if( autorDTO == null ) 
            {
                var autoresDTO = resultado.Value as List<AutorDTO> ?? 
                    throw new ArgumentNullException("Se esperaba una instancia de AutorDTO o listado de AutoresDTO");

                autoresDTO.ForEach(async autor => await generadorEnlaces.GenerarEnlaces(autor));
            }
            else 
            {
                await generadorEnlaces.GenerarEnlaces(autorDTO);
            }           

            await next();
        }
    }
}
