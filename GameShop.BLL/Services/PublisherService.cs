using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.PublisherDTOs;
using GameShop.BLL.Exceptions;
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

        public async Task CreatePublisher(PublisherCreateDTO publisherCreateDTO)
        {
            await _validator.ValidateAndThrowAsync(publisherCreateDTO);
            var newPublisher = _mapper.Map<Publisher>(publisherCreateDTO);
            _unitOfWork.PublisherRepository.Insert(newPublisher);
            await _unitOfWork.SaveAsync();
            _loggerManager.LogInfo($"Publisher created successfully");
        }

        public async Task<PublisherReadDTO> GetPublisherByCompanyName(string companyName)
        {
            var publisher = await _unitOfWork.PublisherRepository.GetAsync(
                filter: p => p.CompanyName == companyName,
                includeProperties: "Games");

            if (publisher.SingleOrDefault() == null)
            {
                throw new NotFoundException($"Publisher with company name {companyName} not found");
            }

            var model = _mapper.Map<PublisherReadDTO>(publisher.SingleOrDefault());
            _loggerManager.LogInfo($"Publisher with company name {companyName} returned successfully");
            return model;
        }
    }
}
