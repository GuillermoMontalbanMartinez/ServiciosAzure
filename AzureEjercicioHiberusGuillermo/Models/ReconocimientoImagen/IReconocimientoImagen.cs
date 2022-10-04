namespace AzureEjercicioHiberusGuillermo.Models.ReconocimientoImagen
{
    public interface IReconocimientoImagen
    {
        public string ImageUrl { get; set; }
        public List<string> Objects { get; set; }
    }
}

