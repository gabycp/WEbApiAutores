namespace WEbApiAutores.DTOs
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int recordsPorPagina = 10;
        private readonly int cantidadMaxPorPagina = 50;

        public int RecordsPorPagina 
        {
            get 
            {
                return recordsPorPagina;
            }

            set 
            {
                recordsPorPagina = (value > cantidadMaxPorPagina) ? cantidadMaxPorPagina : value;
            }
        }
    }
}
