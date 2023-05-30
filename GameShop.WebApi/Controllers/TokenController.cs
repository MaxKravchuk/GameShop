﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GameShop.BLL.DTO.AuthDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/tokens")]
    public class TokenController : ApiController
    {
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IUserService _userService;
        private readonly IUsersTokenService _usersTokenService;

        public TokenController(
            IJwtTokenProvider jwtTokenProvider,
            IUserService userService,
            IUsersTokenService usersTokenService)
        {
            _jwtTokenProvider = jwtTokenProvider;
            _userService = userService;
            _usersTokenService = usersTokenService;
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IHttpActionResult> RefreshAsync(TokenApiModel tokenApiModel)
        {
            if (tokenApiModel is null)
            {
                throw new BadRequestException();
            }

            var accessToken = tokenApiModel.AccessToken;
            var refreshToken = tokenApiModel.RefreshToken;

            var principal = _jwtTokenProvider.ValidateToken(accessToken);
            var userName = principal.Identity.Name;

            if (!await _userService.IsAnExistingUserAsync(userName))
            {
                throw new BadRequestException();
            }

            var role = await _userService.GetRoleAsync(userName);
            var exRefreshToken = await _usersTokenService.GetRefreshTokenAsync(userName);
            if (exRefreshToken.RefreshToken != refreshToken || exRefreshToken.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                throw new BadRequestException();
            }

            var responce = _jwtTokenProvider.GetAuthenticatedResponse(userName, role);
            await _usersTokenService.UpdateUserTokenAsync(userName, responce.RefreshToken);

            return Ok(responce);
        }
    }
}