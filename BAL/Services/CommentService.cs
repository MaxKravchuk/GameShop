using GameShop.BLL.DTO.CommentDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
namespace GameShop.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAsync(CommentCreateDTO newCommentDTO)
        {
            var newComment = _mapper.Map<Comment>(newCommentDTO);
            newComment.GameId = await GetGameIdByKeyAsync(newCommentDTO.GameKey);

            _unitOfWork.CommentRepository.Insert(newComment);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var commentToDelete = await _unitOfWork.CommentRepository.GetByIdAsync(id);
            
            if(commentToDelete != null)
            {
                throw new NotFoundException();
            }

            _unitOfWork.CommentRepository.Delete(commentToDelete);
            await _unitOfWork.SaveAsync();
        }
        public async Task<IEnumerable<CommentReadDTO>> GetAllByGameKeyAsync(string gameKey)
        {
            var comments = await _unitOfWork.CommentRepository.GetAsync(filter: x=>x.Game.Key==gameKey);

            if (!comments.Any())
            {
                throw new NotFoundException();
            }

            var model = _mapper.Map<IEnumerable<CommentReadDTO>>(comments);
            return model;
        }
        public async Task<CommentReadDTO> GetByIdAsync(int commentId)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(commentId);

            if (comment == null)
            {
                throw new NotFoundException();
            }

            var model = _mapper.Map<CommentReadDTO>(comment);
            return model;
        }
        private async Task<int> GetGameIdByKeyAsync(string gameKey)
        {
            if (string.IsNullOrEmpty(gameKey))
            {
                throw new BadRequestException();
            }

            var games = await _unitOfWork.GameRepository.GetAsync(filter: g => g.Key == gameKey);
            var game = games.SingleOrDefault();
            
            return game == null ? throw new BadRequestException() : game.Id;
        }
    }
}
