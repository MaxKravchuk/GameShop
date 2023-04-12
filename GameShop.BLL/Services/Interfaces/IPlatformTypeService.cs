using GameShop.BLL.DTO.PlatformTypeDTOs;
using GameShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IPlatformTypeService
    {
        Task CreateAsync(PlatformTypeCreateDTO platformTypeToAddDTO);
        Task<IEnumerable<PlatformTypeReadListDTO>> GetAsync();
        Task<PlatformTypeReadDTO> GetByIdAsync(int id);
        Task UpdateAsync(PlatformTypeUpdateDTO platformTypeToUpdateDTO);
        Task DeleteAsync(int id);
    }
}
