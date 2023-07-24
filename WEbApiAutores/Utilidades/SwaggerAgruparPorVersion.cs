using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace WEbApiAutores.Utilidades
{
    public class SwaggerAgruparPorVersion : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var namespaceControlador = controller.ControllerType.Namespace; // Controller.V1
            var versionAPI = namespaceControlador.Split('.').Last().ToLower(); // v1
            controller.ApiExplorer.GroupName = versionAPI;
        }
    }
}
