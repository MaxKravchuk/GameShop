using System.Threading.Tasks;
using System.Web.Http;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Filters;

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

        [HttpPost]
        [Route()]
        public async Task<IHttpActionResult> CreateGenreAsync([FromBody] GenreCreateDTO genreCreateDTO)
        {
            await _genreService.CreateAsync(genreCreateDTO);
            return Ok();
        }

        [HttpPut]
        [Route()]
        public async Task<IHttpActionResult> UpdateGenreAsync([FromBody] GenreUpdateDTO genreUpdateDTO)
        {
            await _genreService.UpdateAsync(genreUpdateDTO);
            return Ok();
        }

        [HttpDelete]
        [Route("{genreId}")]
        public async Task<IHttpActionResult> DeleteGenreAsync(int genreId)
        {
            await _genreService.DeleteAsync(genreId);
            return Ok();
        }
    }
}
