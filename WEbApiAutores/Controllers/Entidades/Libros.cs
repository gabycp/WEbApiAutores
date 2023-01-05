namespace WEbApiAutores.Controllers.Entidades
{
    public class Libros
    {
        public int Id { get; set; }
        public string Titulo { get; set; } 
        public int AutorId { get; set; }
        public Autor Autor { get; set; }
       
    }
}
