using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using WEbApiAutores.Controllers;
using WEbApiAutores.Filtros;
using WEbApiAutores.Middlewares;
using WEbApiAutores.Servicios;
using WEbApiAutores.Utilidades;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace WEbApiAutores
{
    public class Startup
    {
        public Startup( IConfiguration configuration )
        {

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

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
                opciones.Conventions.Add( new SwaggerAgruparPorVersion());
            } ).AddJsonOptions( x =>
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();

            service.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer( Configuration.GetConnectionString("defaultConnection")));


            service.AddEndpointsApiExplorer();

            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opciones => opciones.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["llavejwt"])),
                    ClockSkew = TimeSpan.Zero
                });

            

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //service.AddEndpointsApiExplorer();
            service.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "WebApiAutores", 
                    Version = "v1",
                    Description = "Este es un web API para trabajar con autores y libros",
                    Contact = new OpenApiContact 
                    {
                        Email = "gaby_cor93@hotmail.com",
                        Name = "Gabriela Cornejo"
                    },
                    License = new OpenApiLicense 
                    {
                        Name = "MIT"
                    }
                });
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "WebApiAutores", Version = "v2" });
                c.OperationFilter<AgregarParametroHATEOAS>();
                c.OperationFilter<AgregarParametrosXVersion>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },

                        new string[] {}
                    }
                });

                var archivoXML = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var rutaXML = Path.Combine(AppContext.BaseDirectory, archivoXML);
                c.IncludeXmlComments(rutaXML);

                    }) ;

            service.AddAutoMapper(typeof( Startup ));

            
            service.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            service.AddAuthorization(opciones =>
            {
                opciones.AddPolicy("esAdmin", politica => politica.RequireClaim("esAdmin"));
            });

            //Se tiene acceso a los servicios de proteccion de datos
            service.AddDataProtection();

            service.AddCors(opciones => 
            {
                opciones.AddDefaultPolicy(builder => 
                {
                    builder.WithOrigins("https://reqbin.com").AllowAnyMethod().AllowAnyHeader()
                    .WithExposedHeaders(new string[] { "cantidadTotalRegistros" });
                } );
            });

            //Se llama por Transient, ya que este servicio no guarda estado
            service.AddTransient<HashService>();

            //Agregar los servicios para los HATEOAS
            service.AddTransient<GeneradorEnlaces>();
            service.AddTransient<HATEOASAutorFilterAttribute>();
            service.AddSingleton<IActionContextAccessor, ActionContextAccessor>();


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
                //app.UseSwaggerUI(c =>
                //{
                //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiAutores v1");
                //    c.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApiAutores v2");
                    
                //});



            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApiAutores v1");
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "WebApiAutores v2");

            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllers();
            });


        }
    }
}
