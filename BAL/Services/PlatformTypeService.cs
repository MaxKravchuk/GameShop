using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameShop.BLL.DTO.PlatformTypeDTOs;

namespace GameShop.BLL.Services
{
    public class PlatformTypeService : IPlatformTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PlatformTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(PlatformTypeCreateDTO platformTypeToAddDTO)
        {
            var platformTypeToAdd = _mapper.Map<PlatformType>(platformTypeToAddDTO);
            _unitOfWork.PlatformTypeRepository.Insert(platformTypeToAdd);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var platformType = await _unitOfWork.PlatformTypeRepository.GetByIdAsync(id);

            if (platformType == null)
            {
                throw new NotFoundException();
            }

            _unitOfWork.PlatformTypeRepository.Delete(platformType);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<PlatformTypeReadListDTO>> GetAsync()
        {
            var platformTypes = await _unitOfWork.PlatformTypeRepository.GetAsync();

            var platformTypesDTO = _mapper.Map<IEnumerable<PlatformTypeReadListDTO>>(platformTypes);
            return platformTypesDTO;
        }

        public async Task<PlatformTypeReadDTO> GetByIdAsync(int id)
        {
            var platformType = await _unitOfWork.PlatformTypeRepository.GetByIdAsync(id);

            if (platformType == null)
            {
                throw new NotFoundException();
            }

            var platformTypeDTO = _mapper.Map<PlatformTypeReadDTO>(platformType);
            return platformTypeDTO;
        }

        public async Task UpdateAsync(PlatformTypeUpdateDTO platformTypeToUpdateDTO)
        {
            var platformTypeToUpdate = _mapper.Map<PlatformType>(platformTypeToUpdateDTO);

            _unitOfWork.PlatformTypeRepository.Update(platformTypeToUpdate);
            await _unitOfWork.SaveAsync();
        }
    }
}
