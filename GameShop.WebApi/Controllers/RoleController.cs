using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GameShop.BLL.DTO.RoleDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Filters;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/roles")]
    [JwtAuthorize]
    public class RoleController : ApiController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IHttpActionResult> GetAllRolesAsync()
        {
            var roles = await _roleService.GetRolesAsync();
            return Json(roles);
        }

        [HttpPost]
        [Route()]
        public async Task<IHttpActionResult> CreateRoleAsync([FromBody] RoleCreateDTO roleCreateDTO)
        {
            await _roleService.CreateRoleAsync(roleCreateDTO);
            return Ok();
        }

        [HttpPut]
        [Route()]
        public async Task<IHttpActionResult> UpdateRoleAsync([FromBody] RoleUpdateReadListDTO roleUpdateReadListDTO)
        {
            await _roleService.UpdateRoleAsync(roleUpdateReadListDTO);
            return Ok();
        }

        [HttpDelete]
        [Route()]
        public async Task<IHttpActionResult> DeleteRoleAsync(int roleId)
        {
            await _roleService.DeleteRoleAsync(roleId);
            return Ok();
        }
    }
}
