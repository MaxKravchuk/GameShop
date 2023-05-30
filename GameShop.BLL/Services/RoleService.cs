using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.RoleDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;

namespace GameShop.BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly IValidator<RoleBaseDTO> _validator;

        public RoleService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager loggerManager,
            IValidator<RoleBaseDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _validator = validator;
        }

        public async Task CreateRoleAsync(RoleBaseDTO roleBaseDTO)
        {
            await _validator.ValidateAndThrowAsync(roleBaseDTO);

            var role = _mapper.Map<Role>(roleBaseDTO);
            _unitOfWork.RoleRepository.Insert(role);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Role - {roleBaseDTO.Name} created successfully");
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            var roleToDelete = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);

            if (roleToDelete == null)
            {
                throw new NotFoundException($"Role with id {roleId} have not found");
            }

            _unitOfWork.RoleRepository.Delete(roleToDelete);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Role with id {roleId} have been deleted successfully");
        }

        public async Task<RoleReadDTO> GetRoleByIdAsync(int roleId)
        {
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(
                roleId,
                includeProperties: "UsersRole");

            if (role == null)
            {
                throw new NotFoundException($"Role with id {roleId} have not found");
            }

            var model = _mapper.Map<RoleReadDTO>(role);
            _loggerManager.LogInfo(
                $"Role was returned successfully");
            return model;
        }

        public async Task<IEnumerable<RoleUpdateReadListDTO>> GetRolesAsync()
        {
            var roles = await _unitOfWork.RoleRepository.GetAsync();

            var rolesDTO = _mapper.Map<IEnumerable<RoleUpdateReadListDTO>>(roles);

            _loggerManager.LogInfo(
                $"Roles were returned successfully in array size of {rolesDTO.Count()}");
            return rolesDTO;
        }

        public async Task UpdateRoleAsync(RoleUpdateReadListDTO roleUpdateReadListDTO)
        {
            await _validator.ValidateAndThrowAsync(roleUpdateReadListDTO);

            var roleToUpdate = await _unitOfWork.RoleRepository.GetByIdAsync(roleUpdateReadListDTO.Id);

            if (roleToUpdate == null)
            {
                throw new NotFoundException($"User with nickname {roleToUpdate.Name} does not found");
            }

            _mapper.Map(roleUpdateReadListDTO, roleToUpdate);

            _unitOfWork.RoleRepository.Update(roleToUpdate);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Genre with id {roleToUpdate.Id} was updated successfully");
        }
    }
}
