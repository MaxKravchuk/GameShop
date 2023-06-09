using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private readonly IValidator<RoleCreateDTO> _validator;

        public RoleService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager loggerManager,
            IValidator<RoleCreateDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _validator = validator;
        }

        public async Task CreateRoleAsync(RoleCreateDTO roleBaseDTO)
        {
            await _validator.ValidateAndThrowAsync(roleBaseDTO);

            var role = _mapper.Map<Role>(roleBaseDTO);
            _unitOfWork.RoleRepository.Insert(role);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Role - {roleBaseDTO.Name} was created successfully");
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            var roleToDelete = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);

            if (roleToDelete == null)
            {
                throw new NotFoundException($"Role with id {roleId} was not found");
            }

            var users = await _unitOfWork.UserRepository.GetAsync(filter: u => u.UserRole.Id == roleId);

            foreach (var user in users)
            {
                user.RoleId = null;
                _unitOfWork.UserRepository.Update(user);
            }

            _unitOfWork.RoleRepository.HardDelete(roleToDelete);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Role with id {roleId} was deleted successfully");
        }

        public async Task<IEnumerable<RoleReadListDTO>> GetRolesAsync()
        {
            var roles = await _unitOfWork.RoleRepository.GetAsync();

            var rolesDTO = _mapper.Map<IEnumerable<RoleReadListDTO>>(roles);

            _loggerManager.LogInfo(
                $"Roles were returned successfully in array size of {rolesDTO.Count()}");
            return rolesDTO;
        }
    }
}
