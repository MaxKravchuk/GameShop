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
    [RoutePrefix("api/genres")]
    public class GenreController : ApiController
    {
        private readonly IGenreService _genreService;

        public GenreController(
            IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllGenres()
        {
            var result = await _genreService.GetAsync();
            return Json(result);
        }
    }
}
