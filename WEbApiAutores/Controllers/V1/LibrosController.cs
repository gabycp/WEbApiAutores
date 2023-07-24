using AutoMapper;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEbApiAutores.DTOs;
using WEbApiAutores.Entidades;

namespace WEbApiAutores.Controllers.V1
{
    [ApiController]
    [Route("api/v1/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public LibrosController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "obtenerLibro")]
        public async Task<ActionResult<LibroDTOConAutores>> Get(int id)
        {

            var libro = await context.Libros
                //.Include(libroBD => libroBD.Comentarios)
                .Include(libroDB => libroDB.AutoresLibros)
                .ThenInclude(autorLibroDB => autorLibroDB.Autor)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (libro == null)
            {

                return NotFound();
            }

            libro.AutoresLibros = libro.AutoresLibros.OrderBy(x => x.Orden).ToList();
            return mapper.Map<LibroDTOConAutores>(libro);
        }

        [HttpPost(Name = "crearLibro")]
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

        [HttpPut("{Id:int}", Name = "actualizarLibro")]
        public async Task<ActionResult> Put(int libroId, LibroCreacionDTO libroCreacion)
        {
            var libro = await context.Libros
                .Include(libroDB => libroDB.AutoresLibros)
                .FirstOrDefaultAsync(x => x.Id == libroId);

            if (libro == null)
            {
                return NotFound();
            }

            libro = mapper.Map(libroCreacion, libro);

            AsignarOrdenAutores(libro);

            await context.SaveChangesAsync();

            return NoContent();

        }

        private void AsignarOrdenAutores(Libros libro)
        {
            if (libro.AutoresLibros != null)
            {
                for (int i = 0; i < libro.AutoresLibros.Count; i++)
                {
                    libro.AutoresLibros[i].Orden = i;
                }
            }
        }

        [HttpPatch("{id:int}", Name = "patchLibro")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<LibroPatchDTO> jsonPatchDocument)
        {
            if (jsonPatchDocument == null)
            {
                return BadRequest();
            }

            var libroDB = await context.Libros.FirstOrDefaultAsync(x => x.Id == id);

            if (libroDB == null)
            {
                return NotFound();
            }

            var libroDTO = mapper.Map<LibroPatchDTO>(libroDB);

            jsonPatchDocument.ApplyTo(libroDTO, ModelState);

            var esValido = TryValidateModel(libroDTO);

            if (!esValido) { return BadRequest(ModelState); }

            mapper.Map(libroDTO, libroDB);

            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{id:int}", Name = "borrarLibro")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Libros.AnyAsync(x => x.Id == id);

            if (!existe)
            {

                return NotFound();

            }

            context.Remove(new Libros() { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }


    }
}
