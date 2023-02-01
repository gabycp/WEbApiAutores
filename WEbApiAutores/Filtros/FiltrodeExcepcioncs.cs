using Microsoft.AspNetCore.Mvc.Filters;

namespace WEbApiAutores.Filtros
{
    public class FiltrodeExcepcioncs: ExceptionFilterAttribute
    {
        private readonly ILogger<FiltrodeExcepcioncs> logger;

        public FiltrodeExcepcioncs(ILogger<FiltrodeExcepcioncs> logger)
        {
            this.logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError(context.Exception, context.Exception.Message);
            base.OnException(context);

        }
    }
}
