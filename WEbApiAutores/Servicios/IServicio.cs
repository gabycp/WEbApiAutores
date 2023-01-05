namespace WEbApiAutores.Servicios
{
    public interface IServicio
    {
        Guid ObtenerScope();
        Guid ObtenerTransient();
        //Guid ObtenerSingleton();
        void RealizarTarea();
    }
    
    public class ServicioA : IServicio
    {
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScope servicioScope;
        //private readonly ServicioSingleton servicioSingleton;

        public ServicioA(ServicioTransient servicioTransient, ServicioScope servicioScope
                        /*,ServicioSingleton servicioSingleton*/)
        {
            this.servicioTransient = servicioTransient;
            this.servicioScope = servicioScope;
            /*this.servicioSingleton = servicioSingleton;*/
        }

        public Guid ObtenerTransient() {return servicioTransient.Guid;}
        public Guid ObtenerScope() {return servicioScope.Guid;}
        /*public Guid ObtenerSingleton() {return servicioSingleton.Guid;}*/

        public void RealizarTarea() {

            throw new NotImplementedException();
        }

    }

    public class ServicioB : IServicio
    {
        public Guid ObtenerScope()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerSingleton()
        {
            throw new NotImplementedException();
        }

        public Guid ObtenerTransient()
        {
            throw new NotImplementedException();
        }

        public void RealizarTarea()
        {

            throw new NotImplementedException();
        }
    }


    //Creando un string aleatorio
    public class ServicioTransient
    {
        public Guid Guid = Guid.NewGuid();
    }

    public class ServicioScope
    {
        public Guid Guid = Guid.NewGuid();
    }

    /*public class ServicioSingleton
    {
        public Guid Guid = Guid.NewGuid();
    }*/

}
