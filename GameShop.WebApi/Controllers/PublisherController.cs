using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GameShop.BLL.DTO.PublisherDTOs;
using GameShop.BLL.Services.Interfaces;

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
        public async Task<IHttpActionResult> CreatePublisher([FromBody] PublisherCreateDTO publisherCreateDTO)
        {
            await _publisherService.CreatePublisherAsync(publisherCreateDTO);
            return Ok();
        }

        [HttpGet]
        [Route()]
        public async Task<IHttpActionResult> GetPublisherByCompanyName(string companyName)
        {
            var publisher = await _publisherService.GetPublisherByCompanyNameAsync(companyName);
            return Json(publisher);
        }

        [HttpGet]
        [Route("getAll")]
        public async Task<IHttpActionResult> GetAllPublishers()
        {
            var publishers = await _publisherService.GetAllPublishersAsync();
            return Json(publishers);
        }
    }
}
