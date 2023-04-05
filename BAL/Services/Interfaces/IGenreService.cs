using BAL.ViewModels.GameViewModels;
using BAL.ViewModels.GenreViewModels;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IGenreService
    {
        Task Create(Genre genre);
        Task<IEnumerable<Genre>> GetAsync(string gameKey = "");
        Task<Genre> GetByIdAsync(int id);
        Task Update(Genre genre);
        Task Delete(Genre genre);
    }
}
