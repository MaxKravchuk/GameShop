using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GameShop.BLL.DTO.PaginationDTOs;
using GameShop.BLL.DTO.UserDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Filters;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("getAllPaged")]
        [JwtAuthorize(Roles ="Administrator")]
        public async Task<IHttpActionResult> GetAllUsersPagedAsync([FromUri] PaginationRequestDTO paginationRequestDTO)
        {
            var users = await _userService.GetUsersPagedAsync(paginationRequestDTO);
            return Json(users);
        }

        [HttpGet]
        [Route("getAll")]
        [JwtAuthorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetUsersAsync();
            return Json(users);
        }

        [HttpPost]
        [Route("createUserWithRole")]
        [JwtAuthorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> CreateNewUserWithRoleAsync([FromBody] UserCreateWithRoleDTO userWithRoleCreateDTO)
        {
            await _userService.CreateUserWithRoleAsync(userWithRoleCreateDTO);
            return Ok();
        }

        [HttpPut]
        [Route()]
        [JwtAuthorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> UpdateUserAsync([FromBody] UserUpdateDTO userUpdateDTO)
        {
            await _userService.UpdateUserAsync(userUpdateDTO);
            return Ok();
        }

        [HttpDelete]
        [Route("deleteUser/{userId}")]
        [JwtAuthorize(Roles = "Administrator")]
        public async Task<IHttpActionResult> DeleteUserAsync(int userId)
        {
            await _userService.DeleteUserAsync(userId);
            return Ok();
        }

        [HttpGet]
        [Route("isBanned/{nickName}")]
        [JwtAuthorize]
        public async Task<IHttpActionResult> IsUserBanned(string nickName)
        {
            var result = await _userService.IsAnExistingUserBannedAsync(nickName);
            return Ok(result);
        }

        [HttpPut]
        [Route("banUser")]
        [JwtAuthorize(Roles = "Moderator")]
        public async Task<IHttpActionResult> BanUserAsync([FromBody] UserBanDTO userBanDTO)
        {
            await _userService.BanUserAsync(userBanDTO);
            return Ok();
        }
    }
}
