using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WEbApiAutores.Controllers;
using WEbApiAutores.Filtros;
using WEbApiAutores.Middlewares;

namespace WEbApiAutores
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {
            Configuration = configuration;
            ///Se estaban probando las interface y ver la flexibilidad que nos brinda a un nivel alto///
            /*var autorController = new AutoresController(new ApplicationDbContext(null),
                                    new ServicioA()
                                    );*/


        }

        public IConfiguration Configuration { get; }

        public void ConfigureService( IServiceCollection service )
        {
            //Sistema de inyeccion de dependencia
            //Un servicio es la resolucion de una referencia configurada en el sistema de inyecion de dependencia
            //Servicios basados en interfaces

            service.AddControllers( opciones => {
                opciones.Filters.Add(typeof(FiltrodeExcepcioncs));
            } ).AddJsonOptions( x =>
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();

            service.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer( Configuration.GetConnectionString("defaultConnection")));

            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();

            service.AddAutoMapper(typeof( Startup ));

            

        }

        public void Configure( IApplicationBuilder
            
            app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            //app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
            app.UseLoguearRespuestaHTTP();

           

            
            //Los metodos que comienza con Use son los Middleware
            if (env.IsDevelopment()) //Este metodo permite mostrar solo lo que estara en modo de desarrollo y no estara en produccion
            {
                //app.UseSwagger();
               // app.UseSwaggerUI();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllers();
            });


        }
    }
}
