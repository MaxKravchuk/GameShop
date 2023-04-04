using BAL.Exceptions;
using BAL.Services.Interfaces;
using BAL.ViewModels.PlatformTypeViewModels;
using DAL.Entities;
using DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class PlatformTypeService : IPlatformTypeService
    {
        private readonly IRepository<PlatformType> _platformTypeRepository;

        public PlatformTypeService(IUnitOfWork unitOfWork
            )
        {
            _platformTypeRepository = unitOfWork.PlatformTypeRepository;
        }

        public async Task Create(PlatformType platformType)
        {
            _platformTypeRepository.Insert(platformType);
            await _platformTypeRepository.SaveChangesAsync();
        }

        public async Task Delete(PlatformType platformType)
        {
            _platformTypeRepository.Delete(platformType);
            await _platformTypeRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<PlatformType>> GetAsync()
        {
            var plt = await _platformTypeRepository.GetAsync();

            if( plt == null )
            {
                throw new NotFoundException();
            }
            
            return plt;
        }

        public async Task<PlatformType> GetByIdAsync(int id)
        {
            var plt = await _platformTypeRepository.GetByIdAsync(id);

            if( plt == null )
            {
                throw new NotFoundException();
            }

            return plt;
        }

        public async Task Update(PlatformType platformType)
        {
            _platformTypeRepository.Update(platformType);
            await _platformTypeRepository.SaveChangesAsync();
        }
    }
}
