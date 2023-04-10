using GameShop.BLL.DTO.ComentDTOs;
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
        private readonly IGameService _gameService;
        private readonly IMapper _mapper;

        public CommentService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            GameService gameService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _gameService = gameService;
        }

        public async Task CreateAsync(ComentCreateDTO newComentDTO)
        {
            var newComent = _mapper.Map<Comment>(newComentDTO);
            newComent.GameId = (await _gameService.GetGameByKeyAsync(newComentDTO.GameKey)).Id;

            _unitOfWork.ComentRepository.Insert(newComent);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var comentToDelete = await _unitOfWork.ComentRepository.GetByIdAsync(id);
            
            if(comentToDelete != null)
            {
                throw new NotFoundException();
            }

            _unitOfWork.ComentRepository.Delete(comentToDelete);
            await _unitOfWork.SaveAsync();
        }
        public async Task<IEnumerable<ComentReadDTO>> GetAllByGameKeyAsync(string gameKey)
        {
            var coments = await _unitOfWork.ComentRepository.GetAsync(filter: x=>x.Game.Key==gameKey);

            if (coments == null)
            {
                throw new NotFoundException();
            }

            var model = _mapper.Map<IEnumerable<ComentReadDTO>>(coments);
            return model;
        }
        public async Task<ComentReadDTO> GetByIdAsync(int comentId)
        {
            var coment = await _unitOfWork.ComentRepository.GetByIdAsync(comentId);

            if (coment == null)
            {
                throw new NotFoundException();
            }

            var model = _mapper.Map<ComentReadDTO>(coment);
            return model;
        }
    }
}
