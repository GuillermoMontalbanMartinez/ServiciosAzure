using Azure;
using Azure.AI.TextAnalytics;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using AzureEjercicioHiberusGuillermo.Models.ReconocimientoImagen;
using AzureEjercicioHiberusGuillermo.Models.TextoFichero;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Net.Sockets;

namespace AzureEjercicioHiberusGuillermo.Servicios.ReconocimientoImagen
{
    public class ServicioReconocimientoImagen : IServicioReconocimientoImagen
    {
        ReconocimientoImagenModel datos = new();
        private readonly BlobServiceClient _blobClient;
        private readonly string _blobClientKey;
        private readonly IComputerVisionClient _cvClient;
        private readonly TextAnalyticsClient _textAnalyticsClient;
        public IConfiguration Configuration { get; }

        public ServicioReconocimientoImagen()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
            var computerVisionEndPoint = Configuration.GetSection("CognitiveServices:Endpoint").Get<string>();
            var computerVisionKey = Configuration.GetSection("CognitiveServices:Key").Get<string>();
            _blobClient = new BlobServiceClient(Configuration.GetSection("BlobStorage:EndPoint").Get<string>());
            _blobClientKey = Configuration.GetSection("BlobStorage:key").Get<string>();
            _cvClient = new ComputerVisionClient(new Microsoft.Azure.CognitiveServices.Vision.ComputerVision.ApiKeyServiceClientCredentials(computerVisionKey))
            {
                Endpoint = computerVisionEndPoint
            };

        }
        public async Task<List<IReconocimientoImagen>> GetAll()
        {
            var responseList = new List<IReconocimientoImagen>();
            var features = new List<VisualFeatureTypes?>() { VisualFeatureTypes.Objects };

            var containerClient = _blobClient.GetBlobContainerClient("vacaciones");
            var blobs = containerClient.GetBlobsAsync();

            await foreach (var blob in blobs)
            {
                var blobUrl = this.GenerateBlobImageUrl(blob, containerClient);
                var imageAnalysis = await _cvClient.AnalyzeImageAsync(blobUrl, visualFeatures: features, language: "es");

                var datos = new ReconocimientoImagenModel();
                datos.ImageUrl = blobUrl;
                foreach (var obj in imageAnalysis.Objects)
                {
                    datos.Objects.Add(obj.ObjectProperty);
                }

                responseList.Add(datos);
            }

            return responseList;
        }

        private string GenerateBlobImageUrl(BlobItem blob, BlobContainerClient containerClient)
        {
            var sasBuilder = new BlobSasBuilder() { BlobContainerName = containerClient.Name, Resource = blob.Name };
            sasBuilder.ExpiresOn = DateTime.Now.AddMinutes(2);
            sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);
            var sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_blobClient.AccountName, _blobClientKey)).ToString();
            return $"{containerClient.Uri}/{blob.Name}?{sasToken}";
        }

       
    }
}
