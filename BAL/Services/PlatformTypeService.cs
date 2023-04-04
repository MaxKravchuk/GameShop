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

        public void Create()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Task<PlatformTypeReadListViewModel> GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<PlatformType> GetByTypeAsync(string type)
        {
            var plt = _platformTypeRepository.GetAsync(type);

            if( plt == null )
            {
                throw new NotFoundException();
            }

            return plt;
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
