using AzureEjercicioHiberusGuillermo.Models.TextoFichero;

namespace AzureEjercicioHiberusGuillermo.Servicios.TextoFichero
{
    public interface IServicioTextoFichero : IServicios
    {
        Task<ITextoFichero> GetAll();
    }
}
