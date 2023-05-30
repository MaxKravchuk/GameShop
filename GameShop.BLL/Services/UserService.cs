using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.UserDTOs;
using GameShop.BLL.Enums.Extensions;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;

namespace GameShop.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordProvider _passwordProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly IValidator<UserBaseDTO> _validator;

        public UserService(
            IPasswordProvider passwordProvider,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager loggerManager,
            IValidator<UserBaseDTO> validator)
        {
            _passwordProvider = passwordProvider;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _validator = validator;
        }

        public async Task<bool> IsAnExistingUserAsync(string nickName)
        {
            var user = _unitOfWork.UserRepository.GetQuery(filter: u => u.NickName == nickName);
            return await user.SingleOrDefaultAsync() != null;
        }

        public async Task<bool> IsValidUserCredentialsAsync(UserBaseDTO userBaseDTO)
        {
            if (string.IsNullOrEmpty(userBaseDTO.NickName) || string.IsNullOrEmpty(userBaseDTO.Password))
            {
                throw new BadRequestException();
            }

            var user = await _unitOfWork.UserRepository.GetQuery(filter: u => u.NickName == userBaseDTO.NickName)
                .SingleOrDefaultAsync();

            return user != null && _passwordProvider.IsPasswordValid(userBaseDTO.Password, user.PasswordHash);
        }

        public async Task<string> GetRoleAsync(string nickName)
        {
            var user = await _unitOfWork.UserRepository.GetQuery(
                filter: u => u.NickName == nickName,
                includeProperties: "UserRole")
                .SingleOrDefaultAsync();

            if (user == null)
            {
                throw new NotFoundException();
            }

            return user.UserRole.Name;
        }

        public async Task CreateUserAsync(UserCreateDTO userCreateDTO)
        {
            await _validator.ValidateAndThrowAsync(userCreateDTO);

            var user = _mapper.Map<User>(userCreateDTO);
            user.PasswordHash = _passwordProvider.GetPasswordHash(userCreateDTO.Password);
            var role = await _unitOfWork.RoleRepository.GetQuery(filter: x => x.Name == "User").SingleOrDefaultAsync();
            user.RoleId = role.Id;

            _unitOfWork.UserRepository.Insert(user);
            await _unitOfWork.SaveAsync();

            _loggerManager.LogInfo($"User with nickname {user.NickName} created succesfully");
        }

        public async Task CreateUserWithRoleAsync(UserCreateDTO userCreateDTO)
        {
            await _validator.ValidateAndThrowAsync(userCreateDTO);

            var user = _mapper.Map<User>(userCreateDTO);
            user.PasswordHash = _passwordProvider.GetPasswordHash(userCreateDTO.Password);

            var role = await _unitOfWork.RoleRepository.GetQuery(filter: x => x.Name == userCreateDTO.Role)
                .SingleOrDefaultAsync();
            if (role == null)
            {
                throw new BadRequestException("Invalid role");
            }

            user.RoleId = role.Id;

            _unitOfWork.UserRepository.Insert(user);
            await _unitOfWork.SaveAsync();

            _loggerManager.LogInfo($"User with nickname {user.NickName} and roleId {user.RoleId} created succesfully");
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"User with id {userId} does not found");
            }

            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"User with id {userId} was deleted successfully");
        }

        public async Task UpdateUserAsync(UserUpdateDTO userUpdateDTO)
        {
            await _validator.ValidateAndThrowAsync(userUpdateDTO);

            var userToUpdate = await _unitOfWork.UserRepository.GetQuery(filter: x => x.NickName == userUpdateDTO.NickName)
                .SingleOrDefaultAsync();

            if (userToUpdate == null)
            {
                throw new NotFoundException($"User with nickname {userUpdateDTO.NickName} does not found");
            }

            _mapper.Map(userUpdateDTO, userToUpdate);

            _unitOfWork.UserRepository.Update(userToUpdate);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Genre with id {userToUpdate.Id} was updated successfully");
        }

        public async Task<UserReadDTO> GetUserByIdAsync(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(
                userId,
                includeProperties: "UserRole");

            if (user == null)
            {
                throw new NotFoundException($"User with id {userId} does not found");
            }

            var model = _mapper.Map<UserReadDTO>(user);
            _loggerManager.LogInfo(
                $"User was returned successfully");
            return model;
        }

        public async Task<IEnumerable<UserReadListDTO>> GetUsersAsync()
        {
            var users = await _unitOfWork.UserRepository.GetAsync();

            var usersDTO = _mapper.Map<IEnumerable<UserReadListDTO>>(users);

            _loggerManager.LogInfo(
                $"Users were returned successfully in array size of {usersDTO.Count()}");
            return usersDTO;
        }
    }
}
