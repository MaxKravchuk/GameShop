using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.Enums;

namespace GameShop.BLL.Services.Interfaces.Utils
{
    public interface IBlobStorageProvider
    {
        Task<string> UploadAsync(Image image, string fileName, string imageFormat, BlobContainerNames enumBlobContainerName);

        Task DeleteAsync(string imageLink, BlobContainerNames enumBlobContainerName);
    }
}
