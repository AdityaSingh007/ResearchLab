using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureADAuth.Services
{
    public class ImageStore
    {
        CloudBlobClient blobClient;
        string baseUri = "https://researchstorageacct.blob.core.windows.net/";
        private readonly string _accessToken;
        public ImageStore()
        {
            var credentials = new StorageCredentials("researchstorageacct", "vOQOkWB6M+GzNKdJorSg+tx6kZmFYPA7fMiGwbapJG6fp9Jc9jkomOoOIBCJ9ddUiX76/+Loci1RMXzIbMSdMw==");
            blobClient = new CloudBlobClient(new Uri(baseUri), credentials);
        }

        public ImageStore(string accessToken)
        {
            _accessToken = accessToken;
            var tokenCredentials = new TokenCredential(_accessToken);
            var storageCredentials = new StorageCredentials(tokenCredentials);
            blobClient = new CloudBlobClient(new Uri(baseUri), storageCredentials);
        }
        public async Task<string> SaveImage(Stream imageStream)
        {
            var imageId = Guid.NewGuid().ToString();
            try
            {
                var container = blobClient.GetContainerReference("research");
                var blob = container.GetBlockBlobReference(imageId);
                await blob.UploadFromStreamAsync(imageStream);
            }
            catch (StorageException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
            return imageId;
        }


        public string UriFor(string imageId)
        {
            var sasPolicy = new SharedAccessBlobPolicy
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-15),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(15)
            };

            var container = blobClient.GetContainerReference("research");
            var blob = container.GetBlockBlobReference(imageId);
            //var sas = blob.GetSharedAccessSignature(sasPolicy);
            return $"{baseUri}research/{imageId}{_accessToken}";
        }
    }
}
