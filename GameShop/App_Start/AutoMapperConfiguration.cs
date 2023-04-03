using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using AutoMapper;
using BAL.ViewModels.ComentViewModels;
using BAL.ViewModels.GameViewModels;
using BAL.ViewModels.Helpers;
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
            CreateMap<Genre, GameGenreViewModel>();
            CreateMap<PlatformType, GamePlatformTypeViewModel>();
            CreateMap<Game, GameReadViewModel>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.GameGenres.Select(gg => gg.Genre)))
                .ForMember(dest => dest.PlatformTypes, opt => opt.MapFrom(src => src.GamePlatformTypes.Select(gpt => gpt.PlatformType)));
            CreateMap<Game, GameReadListViewModel>();
            CreateMap<GameUpdateViewModel, Game>();
        }

    }
}