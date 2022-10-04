namespace AzureEjercicioHiberusGuillermo.Models.TextoFichero
{
    public class SentimentText : ISentimentText
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public double PositiveScore { get; set; }
        public double NeutralScore { get; set; }
        public double NegativeScore { get; set; }
    }
}
