using System.Collections.Generic;
using System.Threading.Tasks;
using GameShop.BLL.DTO.GenreDTOs;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IGenreService
    {
        Task CreateAsync(GenreCreateDTO genreToAddDTO);

        Task<IEnumerable<GenreReadListDTO>> GetAsync();

        Task UpdateAsync(GenreUpdateDTO genreToUpdateDTO);

        Task DeleteAsync(int id);
    }
}
