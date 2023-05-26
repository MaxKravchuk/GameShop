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
        private readonly ICommentBanService _commentBanService;

        public CommentController(
            ICommentService commentService,
            ICommentBanService commentBanService)
        {
            _commentService = commentService;
            _commentBanService = commentBanService;
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

        [HttpDelete]
        [Route("deleteComment/{commentId}")]
        public async Task<IHttpActionResult> DeleteCommentAsync([FromUri] int commentId)
        {
            await _commentService.DeleteAsync(commentId);
            return Ok();
        }

        [HttpPost]
        [Route("ban")]
        public IHttpActionResult Ban([FromBody] string banDuration)
        {
            _commentBanService.Ban(banDuration);
            return Ok();
        }
    }
}
