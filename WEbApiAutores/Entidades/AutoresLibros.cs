namespace WEbApiAutores.Entidades
{
    public class AutoresLibros
    {
        public int AutorId { get; set; }
        public int LibrosId { get; set; }
        public int Orden { get; set; }
        public Autor Autor { get; set; }
        public Libros Libros { get; set; }
    }
}
