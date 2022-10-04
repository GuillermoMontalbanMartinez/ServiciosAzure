namespace AzureEjercicioHiberusGuillermo.Models.ReconocimientoImagen
{
    public class ReconocimientoImagenModel : IReconocimientoImagen
    {
        public string ImageUrl { get; set; } = null!;
        public List<string> Objects { get; set; } = new List<string>();
    }
}
