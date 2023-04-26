using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.PublisherDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IPublisherService
    {
        Task<PublisherReadDTO> GetPublisherByCompanyNameAsync(string companyName);

        Task CreatePublisherAsync(PublisherCreateDTO publisherCreateDTO);

        Task<IEnumerable<PublisherReadListDTO>> GetAllPublishersAsync();
    }
}
