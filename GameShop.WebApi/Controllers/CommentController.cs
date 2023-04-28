using System.Threading.Tasks;
using System.Web.Http;
using GameShop.BLL.DTO.CommentDTOs;
using GameShop.BLL.Services.Interfaces;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/comments")]
    public class CommentController : ApiController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [Route("leaveComment")]
        public async Task<IHttpActionResult> CreateCommentAsync([FromBody] CommentCreateDTO commentCreateViewModel)
        {
            await _commentService.CreateAsync(commentCreateViewModel);
            return Ok();
        }

        [HttpGet]
        [Route("getAllByGameKey/{gameKey}")]
        public async Task<IHttpActionResult> GetAllCommentsByGameKeyAsync([FromUri] string gameKey)
        {
            var comments = await _commentService.GetAllByGameKeyAsync(gameKey);
            return Json(comments);
        }
    }
}
