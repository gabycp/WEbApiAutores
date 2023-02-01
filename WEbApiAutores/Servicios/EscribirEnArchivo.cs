namespace WEbApiAutores.Servicios
{
    public class EscribirEnArchivo : IHostedService
    {
        private readonly IHostEnvironment env;
        private readonly string nombreArchivo = "Archivo1.txt";

        public EscribirEnArchivo( IHostEnvironment env) 
        {
            this.env = env;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Escribir("Proceso iniciado");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Escribir("Proceso finalizado");
            return Task.CompletedTask;
        }

        private void Escribir(string mensaje) 
        {
            var ruta = $@"{env.ContentRootPath}\wwwroot\{nombreArchivo}";
            using (StreamWriter writer = new StreamWriter(ruta, true))
            {
                writer.WriteLine(mensaje);
            }
        }
    }
}
