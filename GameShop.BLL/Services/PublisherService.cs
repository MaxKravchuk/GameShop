using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.PaginationDTOs;
using GameShop.BLL.DTO.PublisherDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Pagination.Extensions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository.Interfaces;

namespace GameShop.BLL.Services
{
    public class PublisherService : IPublisherService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly IValidator<PublisherCreateDTO> _validator;

        public PublisherService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILoggerManager loggerManager,
            IValidator<PublisherCreateDTO> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _loggerManager = loggerManager;
            _validator = validator;
        }

        public async Task CreatePublisherAsync(PublisherCreateDTO publisherCreateDTO)
        {
            await _validator.ValidateAndThrowAsync(publisherCreateDTO);

            var newPublisher = _mapper.Map<Publisher>(publisherCreateDTO);
            _unitOfWork.PublisherRepository.Insert(newPublisher);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Publisher created successfully");
        }

        public async Task<PublisherReadDTO> GetPublisherByCompanyNameAsync(string companyName)
        {
            var publishers = await _unitOfWork.PublisherRepository.GetAsync(
                filter: p => p.CompanyName == companyName,
                includeProperties: "Games");

            var publisher = publishers.SingleOrDefault();
            if (publisher == null)
            {
                throw new NotFoundException($"Publisher with company name {companyName} not found");
            }

            var model = _mapper.Map<PublisherReadDTO>(publisher);
            _loggerManager.LogInfo($"Publisher with company name {companyName} returned successfully");
            return model;
        }

        public async Task<PublisherReadDTO> GetPublisherByUserIdAsync(int userId)
        {
            var publishers = await _unitOfWork.PublisherRepository.GetAsync(
                   filter: p => p.User.Id == userId,
                   includeProperties: "Games,User");

            var publisher = publishers.SingleOrDefault();
            if (publisher == null)
            {
                throw new NotFoundException($"Publisher with user id {userId} not found");
            }

            var model = _mapper.Map<PublisherReadDTO>(publisher);
            _loggerManager.LogInfo($"Publisher with user id {userId} returned successfully");
            return model;
        }

        public async Task<IEnumerable<PublisherReadListDTO>> GetAllPublishersAsync()
        {
            var publishers = await _unitOfWork.PublisherRepository.GetAsync();

            var models = _mapper.Map<IEnumerable<PublisherReadListDTO>>(publishers);

            _loggerManager.LogInfo($"Publishers successfully returned with array size of {models.Count()}");
            return models;
        }

        public async Task<PagedListDTO<PublisherReadListDTO>> GetAllPublishersPagedAsync(PaginationRequestDTO paginationRequestDTO)
        {
            var publishers = await _unitOfWork.PublisherRepository.GetAsync();

            var pagedPublishers = publishers.ToPagedList(paginationRequestDTO.PageNumber, paginationRequestDTO.PageSize);
            var pagedModels = _mapper.Map<PagedListDTO<PublisherReadListDTO>>(pagedPublishers);

            _loggerManager.LogInfo($"Publishers successfully returned with array size of {pagedModels.Entities.Count()}");
            return pagedModels;
        }

        public async Task UpdatePublisherAsync(PublisherUpdateDTO publisherUpdateDTO)
        {
            await _validator.ValidateAndThrowAsync(publisherUpdateDTO);

            var publisherToUpdate = await _unitOfWork.PublisherRepository.GetByIdAsync(publisherUpdateDTO.Id);
            if (publisherToUpdate == null)
            {
                throw new NotFoundException($"Publisher with id {publisherUpdateDTO.Id} was not found");
            }

            _mapper.Map(publisherUpdateDTO, publisherToUpdate);

            _unitOfWork.PublisherRepository.Update(publisherToUpdate);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Publisher with id {publisherUpdateDTO.Id} updated");
        }

        public async Task DeletePublisherAsync(int publisherId)
        {
            var publisherToDelete = await _unitOfWork.PublisherRepository.GetByIdAsync(publisherId);
            if (publisherToDelete == null)
            {
                throw new NotFoundException($"Publisher with id {publisherId} was not found");
            }

            var games = await _unitOfWork.GameRepository.GetAsync(filter: g => g.PublisherId == publisherId);
            foreach (var game in games)
            {
                game.Publisher = null;
                _unitOfWork.GameRepository.Update(game);
            }

            _unitOfWork.PublisherRepository.Delete(publisherToDelete);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Publisher with id {publisherId} deleted");
        }
    }
}
