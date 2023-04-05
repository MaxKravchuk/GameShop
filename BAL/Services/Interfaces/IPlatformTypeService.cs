using BAL.ViewModels.GameViewModels;
using BAL.ViewModels.PlatformTypeViewModels;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IPlatformTypeService
    {
        Task Create(PlatformType platformType);
        Task<IEnumerable<PlatformType>> GetAsync();
        Task<PlatformType> GetByIdAsync(int id);
        Task Update(PlatformType platformType);
        Task Delete(PlatformType platformType);
    }
}
