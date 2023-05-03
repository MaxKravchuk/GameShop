using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GameShop.BLL.DTO.PlatformTypeDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;

namespace GameShop.BLL.Services
{
    public class PlatformTypeService : IPlatformTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public PlatformTypeService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager loggerManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        public async Task CreateAsync(PlatformTypeCreateDTO platformTypeToAddDTO)
        {
            var platformTypeToAdd = _mapper.Map<PlatformType>(platformTypeToAddDTO);
            _unitOfWork.PlatformTypeRepository.Insert(platformTypeToAdd);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Platform type with type {platformTypeToAddDTO.Type} was created successfully");
        }

        public async Task DeleteAsync(int id)
        {
            var platformType = await _unitOfWork.PlatformTypeRepository.GetByIdAsync(id);

            if (platformType == null)
            {
                throw new NotFoundException($"Platform type with id {id} does not found");
            }

            _unitOfWork.PlatformTypeRepository.Delete(platformType);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Platform type with id {id} was deleted successfully");
        }

        public async Task<IEnumerable<PlatformTypeReadListDTO>> GetAsync()
        {
            var platformTypes = (await _unitOfWork.PlatformTypeRepository.GetAsync()).ToList();

            var platformTypesDTO = _mapper.Map<IEnumerable<PlatformTypeReadListDTO>>(platformTypes);

            _loggerManager.LogInfo(
                $"Platform types were returned successfully in array size of {platformTypesDTO.Count()}");
            return platformTypesDTO;
        }

        public async Task<PlatformTypeReadDTO> GetByIdAsync(int id)
        {
            var platformType = await _unitOfWork.PlatformTypeRepository.GetByIdAsync(id);

            if (platformType == null)
            {
                throw new NotFoundException($"Platform type with id {id} does not found");
            }

            var platformTypeDTO = _mapper.Map<PlatformTypeReadDTO>(platformType);

            _loggerManager.LogInfo($"Platform type with id {id} successfully returned");
            return platformTypeDTO;
        }

        public async Task UpdateAsync(PlatformTypeUpdateDTO platformTypeToUpdateDTO)
        {
            var platformTypeToUpdate = (await _unitOfWork.PlatformTypeRepository.GetAsync(
                filter: plt => plt.Type == platformTypeToUpdateDTO.Type)).SingleOrDefault();

            if (platformTypeToUpdate == null)
            {
                throw new NotFoundException($"Platform type with type {platformTypeToUpdateDTO.Type} does not found");
            }

            _mapper.Map(platformTypeToUpdateDTO, platformTypeToUpdate);

            _unitOfWork.PlatformTypeRepository.Update(platformTypeToUpdate);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Platform type with type {platformTypeToUpdate.Type} was updated successfully");
        }
    }
}
