using System.Threading.Tasks;
using System.Web.Http;
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
        [Route("getAll")]
        public async Task<IHttpActionResult> GetAllGenresAsync()
        {
            var result = await _genreService.GetAsync();
            return Json(result);
        }
    }
}
