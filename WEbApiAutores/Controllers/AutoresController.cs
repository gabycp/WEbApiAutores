using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEbApiAutores.DTOs;
using WEbApiAutores.Entidades;
using System.Linq;

namespace WEbApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    //  [Authorize]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;

        public AutoresController(ApplicationDbContext context, IMapper mapper, IConfiguration configuration) 
        {
            this.context = context;
            this.mapper = mapper;
            this.configuration = configuration;
        }

        //Parte del curso
        //[HttpGet("configuracion")]
        //public ActionResult<string> ObtenerConfiguracion()
        //{
        //    return configuration["ConnectionStrings:defaultConnection"];
        //}


        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<List<AutorDTO>>> Get()
        {
            var autores = await context.Autores.Include(x => x.Libros).ToListAsync();
            return mapper.Map <List<AutorDTO>> (autores);
                
        }

        [HttpGet("{id:int}", Name = "obtenerAutor")]
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id)
        {
            var Autor = await context.Autores
                        .Include(autorDB => autorDB.AutoresLibros)
                        .ThenInclude(autorLibroDB => autorLibroDB.Libros)
                        .FirstOrDefaultAsync(x => x.Id == id);

            if(Autor== null) { return NotFound(); }

            return mapper.Map<AutorDTOConLibros>(Autor);        
        }

        [HttpGet("{nombre}")]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromRoute]string nombre)
        {
            var autores = await context.Autores.Where(x => x.Nombre.Contains(nombre)).ToListAsync();

            
            return mapper.Map<List<AutorDTO>>(autores); 
        }


        [HttpPost]
        public async Task<ActionResult> Post(AutorCreacionDTO autorCreacion)
        {

            var ExisteAutorNombre = await context.Autores.AnyAsync(x => x.Nombre == autorCreacion.Nombre.ToString());

            if (ExisteAutorNombre)
            {
                return BadRequest($"Ya existe el autor con el nombre {autorCreacion.Nombre}");
            }

            var autor = mapper.Map<Autor>(autorCreacion);

            context.Add(autor);
            await context.SaveChangesAsync();

            var autorDTO = mapper.Map<AutorDTO>(autor);

            return CreatedAtRoute("obtenerAutor", new { id = autor.Id }, autorDTO);

        }

        [HttpPut("{id:int}")] //api/autores/1
        public async Task<ActionResult> Put(AutorCreacionDTO autorAutualizado, int id)
        {          

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {

                return NotFound();

            }

            var autor = mapper.Map<Autor>(autorAutualizado);
            autor.Id = id;

            context.Update(autor);
            await context.SaveChangesAsync();
            return NoContent();
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
            return NoContent();
        }
    }
}
