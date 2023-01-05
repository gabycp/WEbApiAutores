using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WEbApiAutores.Controllers;
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
            //app.Use(async (contexto, siguiente) => 
            //{
            //    //Informacion brindad por Raul Liz
            //    // Entrada del middleware
 
            //    // 1. Creo un MemoryStream para poder manipular
            //    // y copiarme el cuerpo de la respuesta.
            //    // Esto se hace porque el stream del cuerpo de la
            //    // respuesta no tiene permisos de lectura.
            //    using var ms = new MemoryStream();
 
            //    // 2. Guardo la referencia del Stream donde se
            //    // escribe el cuerpo de la respuesta
            //    var cuerpoOriginalRespues = contexto.Response.Body;
 
            //    // 3. Cambio el stream por defecto del cuerpo
            //    // de la respuesta por el MemoryStream creado
            //    // para poder manipularlo
            //    contexto.Response.Body = ms;
 
            //    // 4. Esperamos a que el siguiente middleware
            //    // devuelva la respuesta.
            //    await siguiente.Invoke();
 
            //    // Salida del middleware
 
            //    // 5. Nos movemos al principio del MemoryStream
            //    // Para copiar el cuerpo de la respuesta
            //    ms.Seek(0, SeekOrigin.Begin);
 
            //    // 6. Leemos stream hasta el final y almacenamos
            //    // el cuerpo de la respuesta obtenida
            //    var respuesta = new StreamReader(ms).ReadToEnd();
 
            //    // 5. Nos volvemos a posicionar al principio
            //    // del MemoryStream para poder copiarlo al 
            //    // cuerpo original de la respuesta
            //    ms.Seek(0, SeekOrigin.Begin);
 
            //    // 7. Copiamos el contenido del MemoryStream al
            //    // stream original del cuerpo de la respuesta
            //    await ms.CopyToAsync(cuerpoOriginalRespues);
 
            //    // 8.Volvemos asignar el stream original al el cuerpo
            //    // de la respuesta para que siga el flujo normal.
            //    contexto.Response.Body = cuerpoOriginalRespues;
 
            //    // 9. Escribimos en el log la respuesta obtenida
            //    logger.LogInformation(respuesta);
            //});
            
            
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
