using BAL.ViewModels.GameViewModels;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IGameService
    {
        Task Create(Game game);
        Task<IEnumerable<Game>> GetAsync();
        Task<IEnumerable<Game>> GetAsync(string filter);
        Task Update(Game game);
        Task Delete(object id);
    }
}
