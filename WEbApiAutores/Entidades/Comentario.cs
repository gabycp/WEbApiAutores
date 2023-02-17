namespace WEbApiAutores.Entidades
{
    public class Comentario
    {
        public int Id { get; set; }
        public string Contenido { get; set; }
        public int LibrosId { get; set; }
        public Libros Libros { get; set; }
    }
}
