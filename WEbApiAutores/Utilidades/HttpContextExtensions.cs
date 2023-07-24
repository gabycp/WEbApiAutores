using Microsoft.EntityFrameworkCore;

namespace WEbApiAutores.Utilidades
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosPaginacionEnCabecera<T>(this HttpContext httpContext,
                IQueryable<T> querably) 
        {
            if(httpContext == null) throw new ArgumentNullException(nameof(HttpContext));

            double cantidad = await querably.CountAsync();
            httpContext.Response.Headers.Add("cantidadTotalRegistros", cantidad.ToString());

        }
    }
}
