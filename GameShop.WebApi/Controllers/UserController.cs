using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GameShop.BLL.DTO.UserDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Filters;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/users")]
    [JwtAuthorize]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IHttpActionResult> GetAllUsersAsync()
        {
            var users = await _userService.GetUsersAsync();
            return Json(users);
        }

        [HttpPost]
        [Route("createUser")]
        public async Task<IHttpActionResult> CreateNewUserAsync([FromBody] UserCreateDTO userCreateDTO)
        {
            await _userService.CreateUserAsync(userCreateDTO);
            return Ok();
        }

        [HttpPost]
        [Route("createUserWithRole")]
        public async Task<IHttpActionResult> CreateNewUserWithRoleAsync([FromBody] UserWithRoleCreateDTO userWithRoleCreateDTO)
        {
            await _userService.CreateUserWithRoleAsync(userWithRoleCreateDTO);
            return Ok();
        }

        [HttpPut]
        [Route()]
        public async Task<IHttpActionResult> UpdateUserAsync([FromBody] UserUpdateDTO userUpdateDTO)
        {
            await _userService.UpdateUserAsync(userUpdateDTO);
            return Ok();
        }

        [HttpDelete]
        [Route("deleteUser/{userId}")]
        public async Task<IHttpActionResult> DeleteUserAsync(int userId)
        {
            await _userService.DeleteUserAsync(userId);
            return Ok();
        }
    }
}
