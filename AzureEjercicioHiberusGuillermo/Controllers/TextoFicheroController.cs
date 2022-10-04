using Azure.Storage.Blobs;
using AzureEjercicioHiberusGuillermo.Models.TextoFichero;
using AzureEjercicioHiberusGuillermo.Servicios.TextoFichero;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;

namespace AzureEjercicioHiberusGuillermo.Controllers
{
    public class TextoFicheroController : Controller
    {
        private readonly IServicioTextoFichero _TextoFichero;

        public TextoFicheroController(IServicioTextoFichero textFichero)
        {
            _TextoFichero = textFichero;
        }

        // GET: TextoFicheroController
        public async Task<ActionResult> Index()
        {
            var model = await _TextoFichero.GetAll();
            return View(model);
        }

    }
}
