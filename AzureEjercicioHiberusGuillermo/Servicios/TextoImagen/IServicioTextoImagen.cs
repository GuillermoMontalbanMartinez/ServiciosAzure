using AzureEjercicioHiberusGuillermo.Models.TextoFichero;
using AzureEjercicioHiberusGuillermo.Models.TextoImagen;

namespace AzureEjercicioHiberusGuillermo.Servicios.TextoImagen
{
    public interface IServicioTextoImagen
    {
        Task<List<ITextoImagen>> GetAll();
    }
}
