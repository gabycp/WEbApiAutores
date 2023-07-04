using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEbApiAutores.DTOs;
using WEbApiAutores.Entidades;

namespace WEbApiAutores.Controllers
{
    [ApiController]
    [Route("api/libros/{libroId:int}/comentarios")]
    public class ComentariosController: ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ComentariosController(ApplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<ComentarioDT>>> Get(int libroId) 
        { 
            var comentarios = await context.Comentarios.Where(coment => coment.LibrosId == libroId).ToListAsync();

            return mapper.Map<List<ComentarioDT>>(comentarios);
        }

        [HttpGet("{Id:int}", Name = "obtenerComentario")]
        public async Task<ActionResult<ComentarioDT>> GetPorId(int id) 
        {
            var comentarioDTO = await context.Comentarios.FirstOrDefaultAsync( comentarioBD => comentarioBD.Id == id);

            if(comentarioDTO == null)
            {
                return NotFound();
            }

            return mapper.Map<ComentarioDT>(comentarioDTO);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int libroId, ComentarioCreacionDTO comentarioCreacionDTO)
        {
            var email = HttpContext.User.Claims.Where(claim => claim.Type == "email").FirstOrDefault();

            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);

            if (!existeLibro) { 
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacionDTO);
            comentario.LibrosId = libroId;
            context.Add(comentario);
            await context.SaveChangesAsync();

            var comentarioDTO = mapper.Map<ComentarioDT>(comentario);

            return CreatedAtRoute("obtenerComentario", new { id = comentario.Id, libroId = libroId }, comentarioDTO);
            

        }

        [HttpPut("{Id:int}")]
        public async Task<ActionResult> Put(int libroId,int Id, ComentarioCreacionDTO comentarioCreacion)
        {
            var existeLibro = await context.Libros.AnyAsync(x => x.Id == libroId);

            if (!existeLibro)
            {
                return NotFound();
            }

            var existeComentario = await context.Comentarios.AnyAsync(x => x.Id == Id);

            if (!existeLibro)
            {
                return NotFound();
            }

            var comentario = mapper.Map<Comentario>(comentarioCreacion);
            comentario.LibrosId = libroId;
            comentario.Id = Id;

            context.Update(comentario);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }

    
}
