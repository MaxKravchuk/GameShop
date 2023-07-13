using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.DAL.Enums;

namespace GameShop.DAL.Repository.Interfaces.Utils
{
    public interface IBlobStorageProvider
    {
        Task<string> UploadAsync(Image image, string fileName, BlobContainerItemTypes enumBlobContainerName);

        Task DeleteAsync(string imageLink, BlobContainerItemTypes enumBlobContainerName);

        Task<string> UpdateAsync(
            Image updatedImage,
            string existingImageLink,
            string newFileName,
            BlobContainerItemTypes enumBlobContainerItemType);
    }
}
