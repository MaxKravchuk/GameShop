using AutoMapper;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.DTO.CommentDTOs;
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
        public async Task<IHttpActionResult> GetAllCommentsByGameKey([FromUri] string gameKey)
        {
            var comments = await _commentService.GetAllByGameKeyAsync(gameKey);
            return Json(comments);
        }
    }
}