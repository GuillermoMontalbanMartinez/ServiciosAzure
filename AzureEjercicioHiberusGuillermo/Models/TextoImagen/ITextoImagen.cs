namespace AzureEjercicioHiberusGuillermo.Models.TextoImagen
{
    public interface ITextoImagen
    {
        public string ImageUrl { get; set; }
        public List<string> Text { get; set; }
    }
}
