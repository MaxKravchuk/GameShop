﻿using BAL.Exceptions;
using BAL.Services.Interfaces;
using BAL.ViewModels.ComentViewModels;
using DAL.Entities;
using DAL.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services
{
    public class ComentService : IComentService
    {
        private readonly IRepository<Coment> _comentRepository;

        public ComentService(IUnitOfWork unitOfWork)
        {
            _comentRepository = unitOfWork.ComentRepository;
        }

        public async Task Create(Coment coment)
        {
            _comentRepository.Insert(coment);
            await _comentRepository.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var comentToDelete = await _comentRepository.GetByIdAsync(id);
            _comentRepository.Delete(comentToDelete);
            await _comentRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<Coment>> GetAsync()
        {
            var coments = await _comentRepository.GetAsync();
            return coments;
        }
        public async Task<IEnumerable<Coment>> GetAllAsync(string gameKey)
        {
            var coments = await _comentRepository.GetAsync(filter: x=>x.Game.Key==gameKey);

            if (coments == null)
            {
                throw new NotFoundException();
            }

            return coments;
        }
        public async Task<Coment> GetByIdAsync(int comentId)
        {
            var coment = await _comentRepository.GetByIdAsync(comentId);

            if (coment == null)
            {
                throw new NotFoundException();
            }

            return coment;
        }

        public async Task Update(Coment coment)
        {
            _comentRepository.Update(coment);
            await _comentRepository.SaveChangesAsync();
        }
    }
}
