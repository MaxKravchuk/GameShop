using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.CommentDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;

namespace GameShop.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStoredProceduresProvider _storedProceduresProvider;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IValidator<CommentCreateDTO> _validator;

        public CommentService(
            IUnitOfWork unitOfWork,
            IStoredProceduresProvider storedProceduresProvider,
            IMapper mapper,
            ILoggerManager logger,
            IValidator<CommentCreateDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _storedProceduresProvider = storedProceduresProvider;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task CreateAsync(CommentCreateDTO newCommentDTO)
        {
            await _validator.ValidateAndThrowAsync(newCommentDTO);
            var newComment = _mapper.Map<Comment>(newCommentDTO);
            newComment.GameId = await GetGameIdByKeyAsync(newCommentDTO.GameKey);

            if (newCommentDTO.ParentId != null)
            {
                var parentComment = await GetById((int)newCommentDTO.ParentId);
                newComment.Parent = parentComment;
            }

            var user = (await _unitOfWork.UserRepository.GetAsync(
                filter: x => x.NickName == newCommentDTO.Name)).SingleOrDefault();
            if (user == null)
            {
                throw new NotFoundException($"User with nickname {newCommentDTO.Name} not found");
            }

            newComment.User = user;

            _unitOfWork.CommentRepository.Insert(newComment);
            await _unitOfWork.SaveAsync();
            _logger.LogInfo($"Comment for game`s key {newCommentDTO.GameKey} created successfully");
        }

        public async Task DeleteAsync(int id)
        {
            var commentToDelete = await GetById(id);

            _unitOfWork.CommentRepository.Delete(commentToDelete);
            await _unitOfWork.SaveAsync();
            _logger.LogInfo($"Comment with id {id} deleted successfully");
        }

        public async Task<IEnumerable<CommentReadDTO>> GetAllByGameKeyAsync(string gameKey)
        {
            await GetGameIdByKeyAsync(gameKey);
            var comments = await _unitOfWork.CommentRepository.GetAsync(
                filter: x => x.Game.Key == gameKey);
            var model = _mapper.Map<IEnumerable<CommentReadDTO>>(comments);
            _logger.LogInfo($"Comments with game`s key {gameKey} successfully found");
            return model;
        }

        public async Task<Comment> GetById(int commentId)
        {
            var comment = await _storedProceduresProvider.GetCommentByIdAsync(commentId);

            if (comment == null)
            {
                throw new NotFoundException($"Comment with id {commentId} not found");
            }

            _logger.LogInfo($"Comment with id {commentId} successfully found");
            return comment;
        }

        private async Task<int> GetGameIdByKeyAsync(string gameKey)
        {
            if (string.IsNullOrEmpty(gameKey))
            {
                throw new BadRequestException("An empty game key is set");
            }

            var game = await _unitOfWork.GameRepository
                .GetPureQuery(filter: g => g.Key == gameKey).SingleOrDefaultAsync();

            if (game == null)
            {
                throw new NotFoundException($"Game with key {gameKey} not found");
            }

            return game.Id;
        }
    }
}
