using AutoMapper;
using GameShop.BLL.DTO.CommentDTOs;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.DTO.PaginationDTOs;
using GameShop.BLL.DTO.PlatformTypeDTOs;
using GameShop.BLL.DTO.PublisherDTOs;
using GameShop.BLL.Pagination;
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
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Parent, opt => opt.Ignore());

            CreateMap<Comment, CommentReadDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body));

            CreateMap<GameCreateDTO, Game>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.GameGenres, opt => opt.Ignore())
                .ForMember(dest => dest.GamePlatformTypes, opt => opt.Ignore())
                .ForMember(dest => dest.Publisher, opt => opt.Ignore())
                .ForMember(dest => dest.ListOfOrderDetails, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Views, opt => opt.Ignore());

            CreateMap<Genre, GenreReadListDTO>();

            CreateMap<PlatformType, PlatformTypeReadListDTO>();

            CreateMap<Game, GameReadDTO>()
                .ForMember(dest => dest.PlatformTypes, opt => opt.MapFrom(src => src.GamePlatformTypes))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.GameGenres))
                .ForMember(dest => dest.PublisherReadDTO, opt => opt.MapFrom(src => src.Publisher));
            
            CreateMap<Game, GameReadListDTO>();

            CreateMap<PagedList<Game>, PagedListViewModel<GameReadListDTO>>()
                .ForMember(dest => dest.HasNext, opt => opt.MapFrom(src => src.HasNext))
                .ForMember(dest => dest.HasPrevious, opt => opt.MapFrom(src => src.HasPrevious))
                .ForMember(dest => dest.Entities, opt => opt.MapFrom(src => src));

            CreateMap<GameUpdateDTO, Game>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.GameGenres, opt => opt.Ignore())
                .ForMember(dest => dest.GamePlatformTypes, opt => opt.Ignore())
                .ForMember(dest => dest.Publisher, opt => opt.Ignore())
                .ForMember(dest => dest.ListOfOrderDetails, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Views, opt => opt.Ignore());

            CreateMap<PublisherCreateDTO, Publisher>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Games, opt => opt.Ignore());

            CreateMap<Publisher, PublisherReadDTO>()
                .ForMember(dest => dest.GameReadListDTOs, opt => opt.MapFrom(src => src.Games));

            CreateMap<Publisher, PublisherReadListDTO>();

            CreateMap<OrderCreateDTO, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.ListOfOrderDetails, opt => opt.Ignore());
        }
    }
}
