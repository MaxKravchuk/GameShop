﻿using System.Threading.Tasks;
using System.Web.Http;
using GameShop.BLL.DTO.PaginationDTOs;
using GameShop.BLL.DTO.PlatformTypeDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Filters;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/platformTypes")]
    public class PlatformTypeController : ApiController
    {
        private readonly IPlatformTypeService _platformTypeService;

        public PlatformTypeController(
            IPlatformTypeService platformTypeService)
        {
            _platformTypeService = platformTypeService;
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IHttpActionResult> GetAllPlatformTypesAsync()
        {
            var result = await _platformTypeService.GetAsync();
            return Json(result);
        }

        [HttpGet]
        [Route("getAllPaged")]
        public async Task<IHttpActionResult> GetAllPlatformTypesPagedAsync([FromUri] PaginationRequestDTO paginationRequestDTO)
        {
            var result = await _platformTypeService.GetPagedAsync(paginationRequestDTO);
            return Json(result);
        }

        [HttpPost]
        [Route()]
        [JwtAuthorize(Roles = "Manager")]
        public async Task<IHttpActionResult> CreatePlatformTypeAsync([FromBody] PlatformTypeCreateDTO platformTypeCreateDTO)
        {
            await _platformTypeService.CreateAsync(platformTypeCreateDTO);
            return Ok();
        }

        [HttpPut]
        [Route()]
        [JwtAuthorize(Roles = "Manager")]
        public async Task<IHttpActionResult> UpdatePlatformTypeAsync([FromBody] PlatformTypeUpdateDTO platformTypeUpdateDTO)
        {
            await _platformTypeService.UpdateAsync(platformTypeUpdateDTO);
            return Ok();
        }

        [HttpDelete]
        [Route()]
        [JwtAuthorize(Roles = "Manager")]
        public async Task<IHttpActionResult> DeletePlatformTypeAsync(int platformTypeId)
        {
            await _platformTypeService.DeleteAsync(platformTypeId);
            return Ok();
        }
    }
}
