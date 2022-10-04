using AzureEjercicioHiberusGuillermo.Controllers;
using AzureEjercicioHiberusGuillermo.Models.TextoFichero;
using AzureEjercicioHiberusGuillermo.Servicios.ReconocimientoImagen;
using AzureEjercicioHiberusGuillermo.Servicios.TextoFichero;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureEjercicioHiberusGuillermo.Controllers
{
    public class ReconocimientoImagenController : Controller
    {
        private readonly IServicioReconocimientoImagen _ReconocimientoImagen;

        public ReconocimientoImagenController(IServicioReconocimientoImagen reconocimientoImagen)
        {
            _ReconocimientoImagen = reconocimientoImagen;
        }
        // GET: ReconocimientoImagenController
        public async Task<ActionResult> Index()
        {
            var model = await _ReconocimientoImagen.GetAll();
            return View(model);
        }

    }
}
