using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEbApiAutores.DTOs;
using WEbApiAutores.Entidades;

namespace WEbApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name ="obtenerLibro")]
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
        {

            var libro = await context.Libros
                //.Include(libroBD => libroBD.Comentarios)
                .Include(libroDB => libroDB.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();
            return mapper.Map<LibroDTOConAutores>(libro);
        }

        [HttpPost]
        public async Task<ActionResult> Post(LibroCreacionDTO libroDTO)
        {

            if (libroDTO.AutoresId == null)
            {
                return BadRequest("No se puede crear un libro sin autores");
            }

            var AutoresID = await context.Autores.Where(autoresBD => libroDTO.AutoresId.Contains(autoresBD.Id))
                                                 .Select(x => x.Id).ToListAsync();
                

            if (libroDTO.AutoresId.Count != AutoresID.Count)
            {
                return BadRequest("No existe uno de los autores enviado");
            }

            var libro = mapper.Map<Libros>(libroDTO);

            AsignarOrdenAutores(libro);

            context.Add(libro);
            await context.SaveChangesAsync();

            var libritoDTO = mapper.Map<LibroDTO>(libro);

            return CreatedAtRoute("obtenerLibro", new { libro.Id }, libritoDTO);
        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> Put(int libroId, LibroCreacionDTO libroCreacion)
        {
            var libro = await context.Libros
                .Include(libroDB => libroDB.AutoresLibros)
                .FirstOrDefaultAsync(x => x.Id == libroId);

            if(libro == null)
            {
                return NotFound();
            }

            libro = mapper.Map(libroCreacion, libro);

            AsignarOrdenAutores(libro);

            await context.SaveChangesAsync();

            return NoContent();

        }

        public void AsignarOrdenAutores(Libros libro)
        {
            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }
            }
        }
    }
}
