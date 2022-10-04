namespace AzureEjercicioHiberusGuillermo.Models.TextoImagen
{
    public class TextoImagenModel : ITextoImagen
    {
        public string ImageUrl { get; set; }
        public List<string> Text { get; set; } = new List<string>();
    }
}
