using AzureEjercicioHiberusGuillermo.Controllers;
using AzureEjercicioHiberusGuillermo.Models.TextoFichero;
using AzureEjercicioHiberusGuillermo.Servicios.TextoFichero;
using AzureEjercicioHiberusGuillermo.Servicios.TextoImagen;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureEjercicioHiberusGuillermo.Controllers
{
    public class TextoImagenController : Controller
    {
        private readonly IServicioTextoImagen _TextoImagen;

        public TextoImagenController(IServicioTextoImagen textoImagen)
        {
            _TextoImagen = textoImagen;
        }


        // GET: TextoImagenController
        public async Task<ActionResult> Index()
        {
            var model = await _TextoImagen.GetAll();
            return View(model);
        }

    }
}
