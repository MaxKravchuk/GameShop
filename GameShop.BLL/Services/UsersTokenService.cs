using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameShop.BLL.DTO.AuthDTOs;
using GameShop.BLL.DTO.UserDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;

namespace GameShop.BLL.Services
{
    public class UsersTokenService : IUsersTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public UsersTokenService(
            IUnitOfWork unitOfWork,
            IJwtTokenProvider jwtTokenProvider)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenProvider = jwtTokenProvider;
        }

        public async Task<AuthenticatedResponse> AddUserTokenAsync(UserCreateDTO userCreateDTO)
        {
            var user = (await _unitOfWork.UserRepository.GetAsync(
                filter: x => x.NickName == userCreateDTO.NickName,
                includeProperties: "RefreshToken,UserRole"))
                .SingleOrDefault();
            var authResponse = _jwtTokenProvider.GetAuthenticatedResponse(user.Id, user.NickName, user.UserRole.Name);
            var token = user?.RefreshToken;
            if (token != null)
            {
                token.RefreshToken = authResponse.RefreshToken;
                _unitOfWork.UserTokensRepository.Update(token);
                await _unitOfWork.SaveAsync();
                return authResponse;
            }

            var newToken = new UserTokens
            {
                UserId = user.Id,
                User = user,
                RefreshToken = authResponse.RefreshToken,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
            };

            _unitOfWork.UserTokensRepository.Insert(newToken);
            await _unitOfWork.SaveAsync();
            return authResponse;
        }

        public async Task DeleteUserTokenAsync(string nickName)
        {
            var user = (await _unitOfWork.UserRepository.GetAsync(
                filter: x => x.NickName == nickName,
                includeProperties: "RefreshToken"))
                .SingleOrDefault();
            var token = user?.RefreshToken;

            token.RefreshToken = null;
            token.RefreshTokenExpiryTime = default;

            _unitOfWork.UserTokensRepository.Update(token);
            await _unitOfWork.SaveAsync();
        }

        public async Task<AuthenticatedResponse> UpdateUserTokenAsync(string oldRefreshToken)
        {
            var token = (await _unitOfWork.UserTokensRepository.GetAsync(
                filter: x => x.RefreshToken == oldRefreshToken,
                includeProperties: "User")).SingleOrDefault();
            if (token == null)
            {
                throw new NotFoundException("Invalid user nickname");
            }

            if (token.RefreshToken != oldRefreshToken || token.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new BadRequestException("Invalid refresh token data");
            }

            var user = (await _unitOfWork.UserRepository.GetAsync(
                filter: u => u.NickName == token.User.NickName,
                includeProperties: "UserRole")).SingleOrDefault();

            if (user == null)
            {
                throw new NotFoundException($"User with nickname {token.User.NickName} was not found");
            }

            var authResponce = _jwtTokenProvider.GetAuthenticatedResponse(user.Id, user.NickName, user.UserRole.Name);

            token.RefreshToken = authResponce.RefreshToken;

            _unitOfWork.UserTokensRepository.Update(token);
            await _unitOfWork.SaveAsync();

            return authResponce;
        }
    }
}
