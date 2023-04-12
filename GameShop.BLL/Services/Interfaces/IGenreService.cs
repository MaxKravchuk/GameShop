using GameShop.BLL.DTO.GenreDTOs;
using GameShop.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.Services.Interfaces
{
    public interface IGenreService
    {
        Task CreateAsync(GenreCreateDTO genreToAddDTO);
        Task<IEnumerable<GenreReadListDTO>> GetAsync(string gameKey = "");
        Task<GenreReadDTO> GetByIdAsync(int id);
        Task UpdateAsync(GenreUpdateDTO genreToUpdateDTO);
        Task DeleteAsync(int id);
    }
}
