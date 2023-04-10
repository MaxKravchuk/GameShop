using GameShop.BLL.DTO.ComentDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GameShop.BLL.Services.Interfaces
{
    public interface ICommentService
    {
        Task CreateAsync(ComentCreateDTO newComentDTO);
        Task DeleteAsync(int id);
        Task<IEnumerable<ComentReadDTO>> GetAllByGameKeyAsync(string gameKey);
        Task<ComentReadDTO> GetByIdAsync(int comentId);
    }
}
