using AutoMapper;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.DTO.ComentDTOs;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/coments")]
    public class ComentController : ApiController
    {
        private readonly ICommentService _comentService;

        public ComentController(ICommentService comentService)
        {
            _comentService = comentService;
        }

        [HttpPost]
        [Route("leaveComent")]
        public async Task<IHttpActionResult> CreateComentAsync([FromBody] ComentCreateDTO comentCreateViewModel)
        {
            await _comentService.CreateAsync(comentCreateViewModel);
            return Ok();
        }

        [HttpGet]
        [Route("getAllByGameKey/{gameKey}")]
        public async Task<IEnumerable<ComentReadDTO>> GetAllComentsByGameKey([FromUri] string gameKey)
        {
            var coments = await _comentService.GetAllByGameKeyAsync(gameKey);
            return coments;
        }
    }
}