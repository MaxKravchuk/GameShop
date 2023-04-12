using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using AutoMapper;
using GameShop.BLL.DTO.PlatformTypeDTOs;
using GameShop.BLL.DTO.CommentDTOs;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.DAL.Entities;

namespace GameShop.WebApi.App_Start
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<CommentCreateDTO, Comment>();
            CreateMap<Comment, CommentReadDTO>();
            CreateMap<GameCreateDTO, Game>();
            CreateMap<Genre, GenreReadListDTO>();
            CreateMap<PlatformType, PlatformTypeReadListDTO>();
            CreateMap<Game, GameReadDTO>()
                .ForMember(dest=>dest.PlatformTypes, opt=>opt.MapFrom(src=>src.GamePlatformTypes))
                .ForMember(dest=>dest.Genres, opt=>opt.MapFrom(src=>src.GameGenres));
            CreateMap<Game, GameReadListDTO>();
            CreateMap<GameUpdateDTO, Game>();
        }

    }
}