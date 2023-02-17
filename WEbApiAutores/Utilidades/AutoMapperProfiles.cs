using AutoMapper;
using Azure.Core;
using WEbApiAutores.DTOs;
using WEbApiAutores.Entidades;

namespace WEbApiAutores.Utilidades
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles() 
        {
            //Se coloca clase origen a la clase destino
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(autorDTO => autorDTO.libros, opciones => opciones.MapFrom(MapAutorDTOLibros)); ;
            CreateMap<LibroCreacionDTO, Libros>()
                .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
            CreateMap<Libros, LibroDTO>();
            CreateMap<Libros,LibroDTOConAutores>()
                .ForMember(libro => libro.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));
            CreateMap<ComentarioCreacionDTO,Comentario>();
            CreateMap<Comentario, ComentarioDT>();
        }

        private List<LibroDTO> MapAutorDTOLibros(Autor autor, AutorDTO autorDTO)
        {
            var resultado = new List<LibroDTO>();

            if(autor.AutoresLibros == null) { return resultado; }

            foreach (var autorlibro in autor.AutoresLibros)
            {
                resultado.Add(new LibroDTO
                {
                    Id = autorlibro.LibrosId,
                    Titulo = autorlibro.Libros.Titulo
                });
            }

            return resultado;
        }
        private List<AutorDTO> MapLibroDTOAutores(Libros libro, LibroDTO libroDTO)
        {
            var resultado = new List<AutorDTO>();

            if(libro.AutoresLibros == null) { return resultado; }

            foreach (var autorlibro in libro.AutoresLibros)
            {
                resultado.Add(new AutorDTO
                {
                    Id = autorlibro.AutorId,
                    Nombre = autorlibro.Autor.Nombre
                });
            }

            return resultado;
        }

        private List<AutoresLibros> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO, Libros libros) 
        {
            var resultado = new List<AutoresLibros>();

            if(libroCreacionDTO.AutoresId == null) { return resultado; }

            foreach (var autorId in libroCreacionDTO.AutoresId)
            {
                resultado.Add(new AutoresLibros { AutorId = autorId });

            }

            return resultado;
        }
    }
}
