﻿using BAL.ViewModels.GameViewModels;
using BAL.ViewModels.GenreViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IGenreService
    {
        void Create();
        Task<GenreReadListViewModel> GetAsync();
        Task<GenreReadViewModel> GetAsync(string name);
        void Update();
        void Delete();
    }
}
