using System.Threading.Tasks;
using System.Web.Http;
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
        [JwtAuthenticationFilter]
        public async Task<IHttpActionResult> GetAllPlatformTypes()
        {
            var result = await _platformTypeService.GetAsync();
            return Json(result);
        }
    }
}
