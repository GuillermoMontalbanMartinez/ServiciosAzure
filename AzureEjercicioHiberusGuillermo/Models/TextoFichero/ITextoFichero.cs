namespace AzureEjercicioHiberusGuillermo.Models.TextoFichero
{
    public interface ITextoFichero
    {
        public List<SentimentText> Texts { get; set; }
        public int AnalyzedTexts { get; set; }
        public double PositiveCourseAverage { get; set; }
        public double NeutralCourseAverage { get; set; }
        public double NegativeCourseAverage { get; set; }
    }
}
