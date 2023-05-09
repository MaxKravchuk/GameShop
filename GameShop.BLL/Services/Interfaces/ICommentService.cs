using System.Collections.Generic;
using System.Threading.Tasks;
using GameShop.BLL.DTO.CommentDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface ICommentService
    {
        Task CreateAsync(CommentCreateDTO newCommentDTO);

        Task DeleteAsync(int id);

        Task<IEnumerable<CommentReadDTO>> GetAllByGameKeyAsync(string gameKey);

        Task<CommentReadDTO> GetByIdAsync(int commentId);
    }
}
