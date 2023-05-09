using System.Collections.Generic;
using System.Threading.Tasks;
using GameShop.BLL.DTO.PlatformTypeDTOs;

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
