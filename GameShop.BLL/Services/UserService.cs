using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.PaginationDTOs;
using GameShop.BLL.DTO.UserDTOs;
using GameShop.BLL.Enums;
using GameShop.BLL.Enums.Extensions;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Pagination.Extensions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.BLL.Strategies.Interfaces.Factories;
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
        private readonly IValidator<UserCreateDTO> _validator;
        private readonly IBanFactory _banFactory;

        public UserService(
            IPasswordProvider passwordProvider,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager loggerManager,
            IValidator<UserCreateDTO> validator,
            IBanFactory banFactory)
        {
            _passwordProvider = passwordProvider;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _validator = validator;
            _banFactory = banFactory;
        }

        public async Task<bool> IsAnExistingUserAsync(string nickName)
        {
            var user = (await _unitOfWork.UserRepository.GetAsync(filter: u => u.NickName == nickName))
                .SingleOrDefault();
            return user != null;
        }

        public async Task<bool> IsAnExistingUserBannedAsync(string nickName)
        {
            var user = (await _unitOfWork.UserRepository.GetAsync(
                filter: u => u.NickName == nickName)).SingleOrDefault();

            if (user == null)
            {
                throw new NotFoundException($"User with nickname {nickName} was not found");
            }

            return user.BannedTo > DateTime.UtcNow;
        }

        public async Task<bool> IsValidUserCredentialsAsync(UserCreateDTO userCreateDTO)
        {
            if (string.IsNullOrEmpty(userCreateDTO.NickName) || string.IsNullOrEmpty(userCreateDTO.Password))
            {
                throw new BadRequestException("Invalid credentials");
            }

            var user = (await _unitOfWork.UserRepository.GetAsync(filter: u => u.NickName == userCreateDTO.NickName))
                .SingleOrDefault();

            return user != null && _passwordProvider.IsPasswordValid(userCreateDTO.Password, user.PasswordHash);
        }

        public async Task<string> GetRoleAsync(string nickName)
        {
            var user = (await _unitOfWork.UserRepository.GetAsync(
                filter: u => u.NickName == nickName,
                includeProperties: "UserRole")).SingleOrDefault();

            if (user == null)
            {
                throw new NotFoundException($"User with nickname {nickName} was not found");
            }

            return user.UserRole.Name;
        }

        public async Task<int> GetIdAsync(string nickName)
        {
            var user = (await _unitOfWork.UserRepository.GetAsync(
                filter: u => u.NickName == nickName)).SingleOrDefault();

            if (user == null)
            {
                throw new NotFoundException($"User with nickname {nickName} was not found");
            }

            return user.Id;
        }

        public async Task CreateUserAsync(UserCreateDTO userCreateDTO)
        {
            await _validator.ValidateAndThrowAsync(userCreateDTO);

            var user = _mapper.Map<User>(userCreateDTO);
            user.PasswordHash = _passwordProvider.GetPasswordHash(userCreateDTO.Password);
            var role = (await _unitOfWork.RoleRepository.GetAsync(filter: x => x.Name == "User"))
                .SingleOrDefault();
            user.UserRole = role;

            _unitOfWork.UserRepository.Insert(user);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"User with nickname {user.NickName} was created succesfully");
        }

        public async Task CreateUserWithRoleAsync(UserCreateWithRoleDTO userWithRoleCreateDTO)
        {
            await _validator.ValidateAndThrowAsync(userWithRoleCreateDTO);

            var user = _mapper.Map<User>(userWithRoleCreateDTO);
            user.PasswordHash = _passwordProvider.GetPasswordHash(userWithRoleCreateDTO.Password);

            var role = await _unitOfWork.RoleRepository.GetByIdAsync(userWithRoleCreateDTO.RoleId);
            if (role == null)
            {
                throw new BadRequestException("Invalid role");
            }

            if (userWithRoleCreateDTO.PublisherId != null && role.Name == "Publisher")
            {
                var publisher = await _unitOfWork.PublisherRepository
                    .GetByIdAsync((int)userWithRoleCreateDTO.PublisherId);

                user.Publisher = publisher;
            }

            user.UserRole = role;

            _unitOfWork.UserRepository.Insert(user);
            await _unitOfWork.SaveAsync();

            _loggerManager.LogInfo($"User with nickname {user.NickName} created succesfully");
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotFoundException($"User with id {userId} was not found");
            }

            _unitOfWork.UserRepository.Delete(user);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"User with id {userId} was deleted successfully");
        }

        public async Task UpdateUserAsync(UserUpdateDTO userUpdateDTO)
        {
            var userToUpdate = (await _unitOfWork.UserRepository.GetAsync(
                filter: u => u.Id == userUpdateDTO.Id,
                includeProperties: "Publisher,UserRole")).SingleOrDefault();
            var role = await _unitOfWork.RoleRepository.GetByIdAsync(userUpdateDTO.RoleId);

            if (userToUpdate == null || role == null)
            {
                throw new NotFoundException($"User with nickname {userUpdateDTO.Id} or " +
                    $"role with id {userUpdateDTO.RoleId} were not found");
            }

            if (userToUpdate.UserRole.Name == "Publisher" && userToUpdate.UserRole.Name != role.Name)
            {
                userToUpdate.Publisher = null;
                userToUpdate.PublisherId = null;
            }

            userToUpdate.UserRole = role;

            _unitOfWork.UserRepository.Update(userToUpdate);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Genre with id {userToUpdate.Id} was updated successfully");
        }

        public async Task<PagedListDTO<UserReadListDTO>> GetUsersPagedAsync(PaginationRequestDTO paginationRequestDTO)
        {
            var users = await _unitOfWork.UserRepository.GetAsync(includeProperties: "UserRole");

            var pagedUsers = users.ToPagedList(paginationRequestDTO.PageNumber, paginationRequestDTO.PageSize);
            var pagedModels = _mapper.Map<PagedListDTO<UserReadListDTO>>(pagedUsers);

            _loggerManager.LogInfo(
                $"Users were returned successfully in array size of {pagedModels.Entities.Count()}");
            return pagedModels;
        }

        public async Task<IEnumerable<UserReadListDTO>> GetUsersAsync()
        {
            var users = await _unitOfWork.UserRepository.GetAsync(includeProperties: "UserRole");

            var models = _mapper.Map<IEnumerable<UserReadListDTO>>(users);

            _loggerManager.LogInfo(
                $"Users were returned successfully in array size of {models.Count()}");
            return models;
        }

        public async Task BanUserAsync(UserBanDTO userBanDTO)
        {
            var user = (await _unitOfWork.UserRepository.GetAsync(
                filter: x => x.NickName == userBanDTO.NickName)).SingleOrDefault();

            if (!string.IsNullOrEmpty(userBanDTO.BanOption))
            {
                var banOption = userBanDTO.BanOption.ToEnum<BanOptions>();
                var sortingStrategy = _banFactory.GetBanStrategy(banOption);
                user = sortingStrategy.Ban(user);
            }

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveAsync();
        }
    }
}
