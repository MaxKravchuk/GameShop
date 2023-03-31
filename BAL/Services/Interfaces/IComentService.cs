﻿using BAL.ViewModels.ComentViewModels;
using DAL.Entities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IComentService
    {
        Task Create(Coment coment);
        Task Delete(object id);
        Task<IEnumerable<Coment>> GetAsync();
        Task<Coment> GetAsync(int comentId);
        Task Update(Coment coment);
    }
}
