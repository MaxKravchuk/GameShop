﻿using System.Threading.Tasks;
using System.Web.Http;
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
        [Route("getAll")]
        public async Task<IHttpActionResult> GetAllPublishersAsync()
        {
            var publishers = await _publisherService.GetAllPublishersAsync();
            return Json(publishers);
        }

        [HttpPut]
        [Route()]
        public async Task<IHttpActionResult> UpdatePublisherAsync([FromBody] PublisherUpdateDTO publisherUpdateDTO)
        {
            await _publisherService.UpdatePublisherAsync(publisherUpdateDTO);
            return Ok();
        }

        [HttpDelete]
        [Route()]
        public async Task<IHttpActionResult> DeletePublisherAsync(int publisherId)
        {
            await _publisherService.DeletePublisherAsync(publisherId);
            return Ok();
        }
    }
}
