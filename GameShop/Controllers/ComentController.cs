using AutoMapper;
using BAL.Services;
using BAL.Services.Interfaces;
using BAL.ViewModels.ComentViewModels;
using DAL.Entities;
using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GameShop.Controllers
{
    [RoutePrefix("api/coment")]
    public class ComentController : ApiController
    {
        private readonly IComentService _comentService;
        private readonly IMapper _mapper;

        public ComentController(IComentService comentService, IMapper mapper)
        {
            _comentService = comentService;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("leaveComent")]
        public async Task CreateComentAsync([FromBody] ComentCreateViewModel comentCreateViewModel)
        {
            var comentToCrete = _mapper.Map<Coment>(comentCreateViewModel);
            await _comentService.Create(comentToCrete);
        }

        [HttpGet]
        [Route("getAllByGameKey")]
        public async Task<IEnumerable<ComentReadViewModel>> GetAllComentsByGameKey([FromUri] string gameKey)
        {
            var coments = await _comentService.GetAllAsync(gameKey);
            var model = _mapper.Map<IEnumerable<ComentReadViewModel>>(coments);
            return model;
        }
    }
}