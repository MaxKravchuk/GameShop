using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameShop.BLL.DTO.AuthDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;

namespace GameShop.BLL.Services
{
    public class UsersTokenService : IUsersTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UsersTokenService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddUserTokenAsync(string nickName, string refreshToken)
        {
            var token = await _unitOfWork.UserTokensRepository.GetQuery(filter: x => x.User.NickName == nickName)
                .SingleOrDefaultAsync();
            if (token != null)
            {
                await UpdateUserTokenAsync(nickName, refreshToken);
                return;
            }

            var user = await _unitOfWork.UserRepository.GetQuery(filter: x => x.NickName == nickName)
                .SingleOrDefaultAsync();

            var newToken = new UserTokens
            {
                UserId = user.Id,
                User = user,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTime = DateTime.Now.AddDays(7)
            };

            _unitOfWork.UserTokensRepository.Insert(newToken);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteUserTokenAsync(string nickName)
        {
            var user = await _unitOfWork.UserRepository.GetQuery(filter: x => x.NickName == nickName)
                .SingleOrDefaultAsync();

            var token = await _unitOfWork.UserTokensRepository.GetQuery(filter: x => x.UserId == user.Id)
                .SingleOrDefaultAsync();

            token.RefreshToken = null;
            token.RefreshTokenExpiryTime = default;

            _unitOfWork.UserTokensRepository.Update(token);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateUserTokenAsync(string nickName, string refreshToken)
        {
            var user = await _unitOfWork.UserRepository.GetQuery(filter: x => x.NickName == nickName)
                .SingleOrDefaultAsync();

            var token = await _unitOfWork.UserTokensRepository.GetQuery(filter: x => x.UserId == user.Id)
                .SingleOrDefaultAsync();

            token.RefreshToken = refreshToken;

            _unitOfWork.UserTokensRepository.Update(token);
            await _unitOfWork.SaveAsync();
        }

        public async Task<UserTokenReadDTO> GetRefreshTokenAsync(string oldRefreshToken)
        {
            var token = await _unitOfWork.UserTokensRepository.GetQuery(
                filter: x => x.RefreshToken == oldRefreshToken,
                includeProperties: "User").SingleOrDefaultAsync();
            if (token == null)
            {
                throw new NotFoundException("invalid user nickname");
            }

            var model = new UserTokenReadDTO
            {
                RefreshToken = token.RefreshToken,
                RefreshTokenExpiryTime = token.RefreshTokenExpiryTime,
                UserNickName = token.User.NickName
            };
            return model;
        }
    }
}
