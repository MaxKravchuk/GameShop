using BAL.Services.Interfaces;
using BAL.ViewModels.ComentViewModels;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace GameShop.Controllers
{
    [Route("api/home")]
    public class HomeController : ApiController
    {
        private readonly IComentService _comentService;

        public HomeController(IComentService comentService)
        {
            _comentService = comentService;
        }

        [HttpGet]
        public async Task<IEnumerable<Coment>> GetAllAsync()
        {
            var coments = await _comentService.GetAsync();
            return coments;
        }
    }
}
