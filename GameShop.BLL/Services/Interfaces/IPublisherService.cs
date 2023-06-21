using System.Collections.Generic;
using System.Threading.Tasks;
using GameShop.BLL.DTO.PaginationDTOs;
using GameShop.BLL.DTO.PublisherDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IPublisherService
    {
        Task<PublisherReadDTO> GetPublisherByCompanyNameAsync(string companyName);

        Task<PublisherReadDTO> GetPublisherByUserIdAsync(int userId);

        Task CreatePublisherAsync(PublisherCreateDTO publisherCreateDTO);

        Task<IEnumerable<PublisherReadListDTO>> GetAllPublishersAsync();

        Task<PagedListDTO<PublisherReadListDTO>> GetAllPublishersPagedAsync(PaginationRequestDTO paginationRequestDTO);

        Task UpdatePublisherAsync(PublisherUpdateDTO publisherUpdateDTO);

        Task DeletePublisherAsync(int publisherId);
    }
}
