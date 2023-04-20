using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.Services.Interfaces;

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
        public async Task<IHttpActionResult> GetAllGenres()
        {
            var result = await _platformTypeService.GetAsync();
            return Json(result);
        }
    }
}
