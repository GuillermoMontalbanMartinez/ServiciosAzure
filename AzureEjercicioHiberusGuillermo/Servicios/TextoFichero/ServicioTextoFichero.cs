using Azure.AI.TextAnalytics;
using Azure;
using Azure.Storage.Blobs;
using AzureEjercicioHiberusGuillermo.Models.TextoFichero;
using System.Text;
using System.Configuration;

namespace AzureEjercicioHiberusGuillermo.Servicios.TextoFichero
{
    public class ServicioTextoFichero : IServicioTextoFichero
    {
        TextoFicheroModel datos = new();
        private readonly BlobServiceClient _blobClient;
        private readonly string _blobClientKey;
        private readonly TextAnalyticsClient _textAnalyticsClient;
        public IConfiguration Configuration { get; }

        public ServicioTextoFichero()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
            var computerVisionEndPoint = Configuration.GetSection("CognitiveServices:Endpoint").Get<string>();
            var computerVisionKey = Configuration.GetSection("CognitiveServices:Key").Get<string>();
            _blobClient = new BlobServiceClient(Configuration.GetSection("BlobStorage:EndPoint").Get<string>());
            _blobClientKey = Configuration.GetSection("BlobStorage:key").Get<string>();
            _textAnalyticsClient = new TextAnalyticsClient(new Uri(computerVisionEndPoint), new AzureKeyCredential(computerVisionKey));
        }

        public async Task<ITextoFichero> GetAll()
        {
            var containerClient = _blobClient.GetBlobContainerClient("textoarchivo");
            var blobs = containerClient.GetBlobsAsync();

            await foreach (var blob in blobs)
            {
                var blobName = containerClient.GetBlobClient(blob.Name);
                var downloadStream = new MemoryStream();
                blobName.DownloadTo(downloadStream);
                string blobContent = Encoding.UTF8.GetString(downloadStream.ToArray());

                var sentiment = await _textAnalyticsClient.AnalyzeSentimentAsync(blobContent);

                datos.Texts.Add(new SentimentText
                {
                    Text = blobContent,
                    PositiveScore = sentiment.Value.ConfidenceScores.Positive,
                    NeutralScore = sentiment.Value.ConfidenceScores.Neutral,
                    NegativeScore = sentiment.Value.ConfidenceScores.Negative,

                });
            }

            datos.AnalyzedTexts = datos.Texts.Count;
            datos.PositiveCourseAverage = datos.Texts.Sum(x => x.PositiveScore) / datos.Texts.Count;
            datos.NeutralCourseAverage = datos.Texts.Sum(x => x.NeutralScore) / datos.Texts.Count;
            datos.NegativeCourseAverage = datos.Texts.Sum(x => x.NegativeScore) / datos.Texts.Count;

            return datos;
        }
    }
}
