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
    public class ComentController : ApiController
    {
        private readonly IComentService _comentService;

        public ComentController(IComentService comentService)
        {
            _comentService = comentService;
        }

        [HttpGet]
        public async Task<ComentReadViewModel> GetAllAsync()
        {
            var coments = await _comentService.GetAsync();
            var coment = coments.FirstOrDefault();
            var res = new ComentReadViewModel
            {
                Id = coment.Id,
                Body = coment.Body,
                Name = coment.Name
            };
            return res;
        }
    }
}