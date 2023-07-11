using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using GameShop.BLL.Enums;
using GameShop.BLL.Services.Interfaces.Utils;

namespace GameShop.BLL.Services.Utils
{
    public class BlobStorageProvider : IBlobStorageProvider
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _azureBlobContainerName;

        public BlobStorageProvider(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
            _azureBlobContainerName = ConfigurationManager.ConnectionStrings["AzureBlobContainerName"]
                .ConnectionString;
        }

        public async Task<string> UploadAsync(Image image, string fileName, string imageFormat, BlobContainerNames enumBlobContainerName)
        {
            var blobContainerName = enumBlobContainerName.ToString();
            var blobContainer = _blobServiceClient.GetBlobContainerClient(blobContainerName);
            var fileLink = $"{blobContainerName}/{fileName}.{imageFormat}";

            var blobClient = blobContainer.GetBlobClient(fileLink);

            if (await blobClient.ExistsAsync())
            {
                await DeleteAsync(fileLink, enumBlobContainerName);
            }

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                ms.Position = 0;

                await blobClient.UploadAsync(ms);
            }

            fileLink = $"{_azureBlobContainerName}/{blobContainerName.ToString()}/{fileLink}";

            return fileLink;
        }

        public async Task DeleteAsync(string imageLink, BlobContainerNames enumBlobContainerName)
        {
            var blobContainerName = enumBlobContainerName.ToString();
            var blobContainer = _blobServiceClient.GetBlobContainerClient(blobContainerName);

            if (imageLink.Contains(blobContainerName))
            {
                imageLink = imageLink.Split(blobContainerName.ToCharArray())[1];
            }

            var blobClient = blobContainer.GetBlobClient(imageLink);

            await blobClient.DeleteAsync();
        }
    }
}
