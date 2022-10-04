namespace AzureEjercicioHiberusGuillermo.Models.TextoFichero
{
    public class TextoFicheroModel : ITextoFichero
    {
        public List<SentimentText> Texts { get; set; } = new List<SentimentText>();
        public int AnalyzedTexts { get; set; }
        public double PositiveCourseAverage { get; set; }
        public double NeutralCourseAverage { get; set; }
        public double NegativeCourseAverage { get; set; }

    }

    
}
