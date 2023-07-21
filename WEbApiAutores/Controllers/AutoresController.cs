﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEbApiAutores.DTOs;
using WEbApiAutores.Entidades;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WEbApiAutores.Utilidades;

namespace WEbApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "esAdmin")]
    //  [Authorize]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IAuthorizationService authorizationService;

        //private readonly IConfiguration configuration;

        public AutoresController(ApplicationDbContext context, IMapper mapper, 
                                    //IConfiguration configuration,
                                    IAuthorizationService authorizationService ) 
        {
            this.context = context;
            this.mapper = mapper;
            this.authorizationService = authorizationService;
            //this.configuration = configuration;
        }

        //Parte del curso
        //[HttpGet("configuracion")]
        //public ActionResult<string> ObtenerConfiguracion()
        //{
        //    return configuration["ConnectionStrings:defaultConnection"];
        //}

        [HttpGet(Name = "obtenerAutores")] // api/autores 
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<List<AutorDTO>>> Get([FromHeader] string IncluirHATEOAS)
        {
            var autores = await context.Autores.Include(x => x.Libros).ToListAsync();
            
            return mapper.Map <List<AutorDTO>> (autores);

            //if (IncluirHATEOAS)
            //{
            //    var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");
            //    //dto.ForEach(dto => GenerarEnlaces(dto, esAdmin.Succeeded));

            //    var resultado = new ColeccionDeRecursos<AutorDTO> { Valores = dto };
            //    resultado.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("obtenerAutores", new { }),
            //                                            descripcion: "self",
            //                                            metodo: "GET"));
            //    if (esAdmin.Succeeded)
            //    {
            //        resultado.Enlaces.Add(new DatoHATEOAS(enlace: Url.Link("crearAutor", new { }),
            //                                            descripcion: "crear-autor",
            //                                            metodo: "POST"));
            //    }

            //    return Ok(resultado);
            //}

            //return Ok(dto);
                
        }

        [HttpGet("{id:int}", Name = "obtenerAutor")]
        [AllowAnonymous]
        [ServiceFilter(typeof(HATEOASAutorFilterAttribute))]
        public async Task<ActionResult<AutorDTOConLibros>> Get(int id, [FromHeader] string IncluirHATEOAS)
        {
            var Autor = await context.Autores
                        .Include(autorDB => autorDB.AutoresLibros)
                        .ThenInclude(autorLibroDB => autorLibroDB.Libros)
                        .FirstOrDefaultAsync(x => x.Id == id);

            if(Autor== null) { return NotFound(); }

            //var esAdmin = await authorizationService.AuthorizeAsync(User, "esAdmin");
            var dto = mapper.Map<AutorDTOConLibros>(Autor);  
            
            //GenerarEnlaces(dto, esAdmin.Succeeded);

            return dto;
        }

        //private void GenerarEnlaces(AutorDTO autorDTO, bool esAdmin) 
        //{
        //    autorDTO.Enlaces.Add(new DatoHATEOAS( 
        //        enlace: Url.Link("obtenerAutor", new { id = autorDTO.Id }),
        //        descripcion:"self",
        //        metodo:"GET"));

        //    if(esAdmin)
        //    {
        //        autorDTO.Enlaces.Add(new DatoHATEOAS(
        //        enlace: Url.Link("actualizarAutor", new { id = autorDTO.Id }),
        //        descripcion: "self",
        //        metodo: "PUT"));

        //        autorDTO.Enlaces.Add(new DatoHATEOAS(
        //            enlace: Url.Link("borrarAutor", new { id = autorDTO.Id }),
        //            descripcion: "self",
        //            metodo: "DELETE"));
        //    }
            
        //}

        [HttpGet("{nombre}", Name ="obtenerAutorPorNombre")]
        public async Task<ActionResult<List<AutorDTO>>> GetAutorPorNombre([FromRoute]string nombre)
        {
            var autores = await context.Autores.Where(x => x.Nombre.Contains(nombre)).ToListAsync();

            
            return mapper.Map<List<AutorDTO>>(autores); 
        }


        [HttpPost(Name = "crearAutor")]
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

        [HttpPut("{id:int}", Name = "actualizarAutor")] //api/autores/1
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

        [HttpDelete("{id:int}", Name = "borrarAutor")] //api/autores/2
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
