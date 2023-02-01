using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEbApiAutores.Controllers.Entidades;
using WEbApiAutores.Servicios;

namespace WEbApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    //  [Authorize]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicio servicio;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScope servicioScope;
        //private readonly ServicioSingleton servicioSingleton;
        private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationDbContext context, IServicio servicio,
                                 ServicioTransient servicioTransient, ServicioScope servicioScope,
                                 /*ServicioSingleton servicioSingleton,*/ ILogger<AutoresController> logger) //Ver Readme.md para concepto Ilogger
        {
            this.context = context;
            this.servicio = servicio;
            this.servicioTransient = servicioTransient;
            this.servicioScope = servicioScope;
            //this.servicioSingleton = servicioSingleton;
            this.logger = logger;
        }

        [HttpGet("GUID")]
        [ResponseCache(Duration = 10)]
        public ActionResult ObtenerGuids()
        {
            return Ok(new
            {
                AutoresController_Transient = servicioTransient.Guid,
                AutoresController_Scope = servicioScope.Guid,
                //AutoresController_Singleton = servicioSingleton.Guid,
                ServicioA_Transient = servicio.ObtenerTransient(),
                ServicioA_Scope = servicio.ObtenerScope(),
                //ServicioA_Singleton = servicio.ObtenerSingleton(),

            });
        }

        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            logger.LogInformation("Estanis obteniendo los autores");
            logger.LogWarning("Este es un mensaje de prueba");
            return await context.Autores.Include( x=> x.Libros ).ToListAsync();
                
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> Get(int id)
        {
            return await context.Autores.FirstOrDefaultAsync(x => x.Id == id);        
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<Autor>> Get(string nombre)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Nombre.Contains(nombre));

            if(autor == null) { return NotFound(); }

            return autor; 
        }

        [HttpGet("GetPrimerAutor")]
        public async Task<ActionResult<Autor>> PrimerAutor()
        {
            return await context.Autores.FirstOrDefaultAsync();
        }

        [HttpGet("GetPrimero")]
        public async Task<ActionResult<Autor>> PrimerA([FromHeader] int miValor )
        {
            return await context.Autores.FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {

            var ExisteAutorNombre = await context.Autores.AnyAsync(x => x.Equals(autor.Nombre));

            if (ExisteAutorNombre)
            {
                return BadRequest($"Ya existe el autor con el nombre {autor.Nombre}");
            }

            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();

        }

        [HttpPut("{id:int}")] //api/autores/1
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id) {

                return BadRequest("El id del autor no coiciden con el id de la URL");
            
            }

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {

                return NotFound();

            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")] //api/autores/2
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {

                return NotFound();

            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
