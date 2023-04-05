using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using AutoMapper;
using BAL.ViewModels.ComentViewModels;
using BAL.ViewModels.GameViewModels;
using BAL.ViewModels.GenreViewModels;
using BAL.ViewModels.Helpers;
using BAL.ViewModels.PlatformTypeViewModels;
using DAL.Entities;

namespace GameShop.App_Start
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<ComentCreateViewModel, Coment>();
            CreateMap<Coment, ComentReadViewModel>();
            CreateMap<GameCreateViewModel, Game>();
            CreateMap<Genre, GenreReadListViewModel>();
            CreateMap<PlatformType, PlatformTypeReadListViewModel>();
            CreateMap<Game, GameReadViewModel>()
                .ForMember(dest=>dest.PlatformTypes, opt=>opt.MapFrom(src=>src.GamePlatformTypes))
                .ForMember(dest=>dest.Genres, opt=>opt.MapFrom(src=>src.GameGenres));
            CreateMap<Game, GameReadListViewModel>();
            CreateMap<GameUpdateViewModel, Game>();
        }

    }
}