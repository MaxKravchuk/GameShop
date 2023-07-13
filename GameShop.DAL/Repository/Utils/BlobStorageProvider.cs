using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GameShop.BLL.Extensions;
using GameShop.DAL.Enums;
using GameShop.DAL.Repository.Interfaces.Utils;
using Microsoft.Azure.Storage.Blob;

namespace GameShop.DAL.Repository.Utils
{
    public class BlobStorageProvider : IBlobStorageProvider
    {
        private const string ImageFormat = "jpg";
        private readonly CloudBlobClient _blobClient;
        private readonly string _azureBlobContainerName;
        private readonly string _azureBlobContainerLink;

        public BlobStorageProvider(CloudBlobClient blobClient)
        {
            _blobClient = blobClient;
            _azureBlobContainerName = ConfigurationManager.ConnectionStrings["AzureBlobContainerName"]
                .ConnectionString;
            _azureBlobContainerLink = ConfigurationManager.ConnectionStrings["AzureBlobContainerLink"]
                .ConnectionString;
        }

        public async Task<string> UploadAsync(Image image, string fileName, BlobContainerItemTypes enumBlobContainerItemType)
        {
            var blobContainerItemType = enumBlobContainerItemType.ToString();
            var blobContainer = _blobClient.GetContainerReference(_azureBlobContainerName);
            var fileLink = $"{blobContainerItemType}/{fileName}-{Guid.NewGuid()}.{ImageFormat}";

            var blobItmes = await blobContainer.ListBlobsSegmentedAsync($"{blobContainerItemType}/{fileName}", null);

            if (blobItmes.Results.Any())
            {
                await DeleteAsync(blobItmes.Results.First().Uri.ToString(), enumBlobContainerItemType);
            }

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, image.RawFormat);
                    ms.Position = 0;

                    var blockBlob = blobContainer.GetBlockBlobReference(fileLink);
                    await blockBlob.UploadFromStreamAsync(ms);
                }
            }
            catch (Exception ex)
            {
                throw new AggregateException($"An error occurred while adding a photo: {ex.Message}");
            }

            fileLink = $"{_azureBlobContainerLink}/{_azureBlobContainerName}/{fileLink}";

            return fileLink;
        }

        public async Task DeleteAsync(string imageLink, BlobContainerItemTypes enumBlobContainerItemType)
        {
            var blobContainerItemType = enumBlobContainerItemType.ToString();
            var blobContainer = _blobClient.GetContainerReference(_azureBlobContainerName);
            var blobFolder = blobContainer.GetDirectoryReference(blobContainerItemType);

            if (imageLink.Contains(blobContainerItemType))
            {
                imageLink = imageLink.SplitString(blobContainerItemType)[1].Trim('/');
            }

            var blockBlob = blobFolder.GetBlockBlobReference(imageLink);

            await blockBlob.DeleteAsync();
        }

        public async Task<string> UpdateAsync(
            Image updatedImage,
            string existingImageLink,
            string newFileName,
            BlobContainerItemTypes enumBlobContainerItemType)
        {
            if (!string.IsNullOrEmpty(existingImageLink))
            {
                await DeleteAsync(existingImageLink, enumBlobContainerItemType);
            }

            var updatedFileLink = await UploadAsync(updatedImage, newFileName, enumBlobContainerItemType);
            return updatedFileLink;
        }
    }
}
