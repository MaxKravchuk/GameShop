using System.Collections.Generic;
using System.Threading.Tasks;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.DTO.PaginationDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IGenreService
    {
        Task CreateAsync(GenreCreateDTO genreToAddDTO);

        Task<IEnumerable<GenreReadListDTO>> GetAsync();

        Task<PagedListDTO<GenreReadListDTO>> GetPagedAsync(PaginationRequestDTO paginationRequestDTO);

        Task UpdateAsync(GenreUpdateDTO genreToUpdateDTO);

        Task DeleteAsync(int id);
    }
}
