using BAL.ViewModels.GameViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IGameService
    {
        void Create();
        Task<GameReadListViewModel> GetAsync();
        Task<GameReadViewModel> GetAsync(string key);
        void Update();
        void Delete();
    }
}
