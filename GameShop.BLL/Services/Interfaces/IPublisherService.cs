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
        Task<PublisherReadDTO> GetPublisherByCompanyName(string companyName);

        Task CreatePublisher(PublisherCreateDTO publisherCreateDTO);
    }
}
