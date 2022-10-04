using AzureEjercicioHiberusGuillermo.Models.ReconocimientoImagen;
using AzureEjercicioHiberusGuillermo.Models.TextoFichero;

namespace AzureEjercicioHiberusGuillermo.Servicios.ReconocimientoImagen
{
    public interface IServicioReconocimientoImagen : IServicios
    {
        Task<List<IReconocimientoImagen>> GetAll();
    }
}
