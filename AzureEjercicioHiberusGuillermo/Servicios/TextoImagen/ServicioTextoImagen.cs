using Azure.AI.TextAnalytics;
using Azure.Storage.Blobs;
using Azure;
using AzureEjercicioHiberusGuillermo.Models.TextoFichero;
using System.Text;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System.Net.Sockets;
using AzureEjercicioHiberusGuillermo.Models.TextoImagen;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Azure.Storage;

namespace AzureEjercicioHiberusGuillermo.Servicios.TextoImagen
{
    public class ServicioTextoImagen : IServicioTextoImagen
    {
        TextoImagenModel datos = new();
        private readonly BlobServiceClient _blobClient;
        private readonly string _blobClientKey;
        private readonly TextAnalyticsClient _textAnalyticsClient;
        private readonly IComputerVisionClient _cvClient;
        public IConfiguration Configuration { get; }

        public ServicioTextoImagen()
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

        public async Task<List<ITextoImagen>> GetAll()
        {
            var responseList = new List<ITextoImagen>();

            var containerClient = _blobClient.GetBlobContainerClient("textofoto");
            var blobs = containerClient.GetBlobsAsync();

            await foreach (var blob in blobs)
            {
                var blobUrl = this.GenerateBlobImageUrl(blob, containerClient);
                var textHeaders = await _cvClient.ReadAsync(blobUrl);
                string operationLocation = textHeaders.OperationLocation;
                string operationId = operationLocation.Substring(operationLocation.Length - 36);

                var response = new TextoImagenModel();
                response.ImageUrl = blobUrl;

                ReadOperationResult results;
                do
                {
                    results = await _cvClient.GetReadResultAsync(Guid.Parse(operationId));
                }
                while ((results.Status == OperationStatusCodes.Running ||
                        results.Status == OperationStatusCodes.NotStarted));

                var textUrlFileResults = results.AnalyzeResult.ReadResults;
                foreach (ReadResult page in textUrlFileResults)
                {
                    foreach (Line line in page.Lines)
                    {
                        response.Text.Add(line.Text);
                    }
                }
                responseList.Add(response);
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
