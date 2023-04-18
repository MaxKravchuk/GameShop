using AutoMapper;
using GameShop.BLL.DTO.CommentDTOs;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.DTO.PlatformTypeDTOs;
using GameShop.DAL.Entities;

namespace GameShop.WebApi.App_Start
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<CommentCreateDTO, Comment>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Game, opt => opt.Ignore())
                .ForMember(dest => dest.GameId, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<Comment, CommentReadDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body));

            CreateMap<GameCreateDTO, Game>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.GameGenres, opt => opt.Ignore())
                .ForMember(dest => dest.GamePlatformTypes, opt => opt.Ignore());

            CreateMap<Genre, GenreReadListDTO>();

            CreateMap<PlatformType, PlatformTypeReadListDTO>();

            CreateMap<Game, GameReadDTO>()
                .ForMember(dest => dest.PlatformTypes, opt => opt.MapFrom(src => src.GamePlatformTypes))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.GameGenres));
            CreateMap<Game, GameReadListDTO>();

            CreateMap<GameUpdateDTO, Game>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.GameGenres, opt => opt.Ignore())
                .ForMember(dest => dest.GamePlatformTypes, opt => opt.Ignore());
        }
    }
}
