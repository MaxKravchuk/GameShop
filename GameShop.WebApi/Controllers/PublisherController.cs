using System.Threading.Tasks;
using System.Web.Http;
using GameShop.BLL.DTO.PaginationDTOs;
using GameShop.BLL.DTO.PublisherDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Filters;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/publishers")]
    public class PublisherController : ApiController
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpPost]
        [Route()]
        [JwtAuthorize(Roles = "Manager")]
        public async Task<IHttpActionResult> CreatePublisherAsync([FromBody] PublisherCreateDTO publisherCreateDTO)
        {
            await _publisherService.CreatePublisherAsync(publisherCreateDTO);
            return Ok();
        }

        [HttpGet]
        [Route()]
        public async Task<IHttpActionResult> GetPublisherByCompanyNameAsync(string companyName)
        {
            var publisher = await _publisherService.GetPublisherByCompanyNameAsync(companyName);
            return Json(publisher);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IHttpActionResult> GetPublisherByUserIdAsync(int userId)
        {
            var publisher = await _publisherService.GetPublisherByUserIdAsync(userId);
            return Json(publisher);
        }

        [HttpGet]
        [Route("getAllPaged")]
        public async Task<IHttpActionResult> GetAllPublishersPagedAsync([FromUri] PaginationRequestDTO paginationRequestDTO)
        {
            var publishers = await _publisherService.GetAllPublishersPagedAsync(paginationRequestDTO);
            return Json(publishers);
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IHttpActionResult> GetAllPublishersAsync()
        {
            var publishers = await _publisherService.GetAllPublishersAsync();
            return Json(publishers);
        }

        [HttpPut]
        [Route()]
        [JwtAuthorize(Roles = "Manager, Publisher")]
        public async Task<IHttpActionResult> UpdatePublisherAsync([FromBody] PublisherUpdateDTO publisherUpdateDTO)
        {
            await _publisherService.UpdatePublisherAsync(publisherUpdateDTO);
            return Ok();
        }

        [HttpDelete]
        [Route()]
        [JwtAuthorize(Roles = "Manager")]
        public async Task<IHttpActionResult> DeletePublisherAsync(int publisherId)
        {
            await _publisherService.DeletePublisherAsync(publisherId);
            return Ok();
        }
    }
}
