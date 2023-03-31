﻿using BAL.ViewModels.GameViewModels;
using BAL.ViewModels.PlatformTypeViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Interfaces
{
    public interface IPlatformTypeService
    {
        void Create();
        Task<PlatformTypeReadListViewModel> GetAsync();
        Task<PlatformTypeReadViewModel> GetAsync(string type);
        void Update();
        void Delete();
    }
}
