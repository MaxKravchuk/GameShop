using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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

        public UserService(
            IPasswordProvider passwordProvider,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager loggerManager)
        {
            _passwordProvider = passwordProvider;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        public async Task<bool> IsAnExistingUser(string nickName)
        {
            var user = _unitOfWork.UserRepository.GetQuery(filter: u => u.NickName == nickName);
            return await user.SingleOrDefaultAsync() != null;
        }

        public async Task<bool> IsValidUserCredentials(UserBaseDTO userBaseDTO)
        {
            if (string.IsNullOrEmpty(userBaseDTO.NickName) || string.IsNullOrEmpty(userBaseDTO.Password))
            {
                throw new BadRequestException();
            }

            var user = await _unitOfWork.UserRepository.GetQuery(filter: u => u.NickName == userBaseDTO.NickName)
                .SingleOrDefaultAsync();

            return user != null && _passwordProvider.IsPasswordValid(userBaseDTO.Password, user.PasswordHash);
        }

        public async Task CreateUser(UserCreateDTO userCreateDTO)
        {
            // TODO Add validation
            var user = _mapper.Map<User>(userCreateDTO);
            user.PasswordHash = _passwordProvider.GetPasswordHash(userCreateDTO.Password);
            var role = await _unitOfWork.RoleRepository.GetQuery(filter: x => x.Name == "Administrator").SingleOrDefaultAsync();
            user.RoleId = role.Id;
            _unitOfWork.UserRepository.Insert(user);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo("User created");
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

        public void DeleteUser(string login)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(UserUpdateDTO userUpdateDTO)
        {
            throw new NotImplementedException();
        }
    }
}
