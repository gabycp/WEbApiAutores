using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WEbApiAutores.Controllers;
using WEbApiAutores.Middlewares;
using WEbApiAutores.Servicios;

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

            service.AddControllers().AddJsonOptions( x =>
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

            service.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer( Configuration.GetConnectionString("defaultConnection")));

            //Servicio de tipo transitorio
            //Dos formas de configurar tanto una interface como servicio o como una clase
            //Se usa para simples funciones, sin mantener data, ni estado o datos de usuarios.
            service.AddTransient<IServicio, ServicioA>();
            //service.AddTransient<ServicioA>();

            //Existen diferentes tipos de servicios
            //La clase Scope es que la clase de vida aumenta para tener instancias distinta del cliente realizando la peticion
            //Utili de iniciar un patron de trabajo donde se puede tener en varias partes de la aplicacion realizando varios cambio en el servicio
            // y al final decidir si esos cambios van a aplicar o no
            service.AddScoped<IServicio, ServicioA>();

            //Siempre se tendra la misma instancia, se comparte la misma instancia, pero en distinda peticiones
            //se puede utilizar con data para que sea compartida para todos
            //service.AddSingleton<IServicio, ServicioA>();

            service.AddTransient<ServicioTransient>();
            service.AddScoped<ServicioScope>();
            //service.AddSingleton<ServicioSingleton>();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();

            

        }

        public void Configure( IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            //app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
            app.UseLoguearRespuestaHTTP();

            //Si se quiere ir a una ruta especifica se utiliza Map
            app.Map("/ruta1", app => {

                //Con Run se puede tener la oportunidad de crear middleware y cortar la ejecución de los proximos middleware
                app.Run(async contexto => {
                    await contexto.Response.WriteAsync("Estoy interceptando la tubería");
                });

            });

            
            //Los metodos que comienza con Use son los Middleware
            if (env.IsDevelopment()) //Este metodo permite mostrar solo lo que estara en modo de desarrollo y no estara en produccion
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

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
