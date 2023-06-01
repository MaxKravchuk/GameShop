using System.Collections.Generic;
using System.Threading.Tasks;
using GameShop.BLL.DTO.PublisherDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IPublisherService
    {
        Task<PublisherReadDTO> GetPublisherByCompanyNameAsync(string companyName);

        Task CreatePublisherAsync(PublisherCreateDTO publisherCreateDTO);

        Task<IEnumerable<PublisherReadListDTO>> GetAllPublishersAsync();

        Task UpdatePublisherAsync(PublisherUpdateDTO publisherUpdateDTO);

        Task DeletePublisherAsync(int publisherId);
    }
}
