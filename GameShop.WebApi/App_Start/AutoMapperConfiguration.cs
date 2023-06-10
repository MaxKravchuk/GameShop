using AutoMapper;
using GameShop.BLL.DTO.CommentDTOs;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.DTO.OrderDetailDTOs;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.DTO.PaginationDTOs;
using GameShop.BLL.DTO.PlatformTypeDTOs;
using GameShop.BLL.DTO.PublisherDTOs;
using GameShop.BLL.DTO.RoleDTOs;
using GameShop.BLL.DTO.UserDTOs;
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
                .ForMember(dest => dest.Parent, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

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

            CreateMap<Genre, GenreReadListDTO>()
                .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.ParentGenreId));

            CreateMap<GenreCreateDTO, Genre>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.GameGenres, opt => opt.Ignore())
                .ForMember(dest => dest.ParentGenre, opt => opt.Ignore())
                .ForMember(dest => dest.SubGenres, opt => opt.Ignore());

            CreateMap<PagedList<Genre>, PagedListDTO<GenreReadListDTO>>()
                .ForMember(dest => dest.Entities, opt => opt.MapFrom(src => src));

            CreateMap<PlatformType, PlatformTypeReadListDTO>();

            CreateMap<PagedList<PlatformType>, PagedListDTO<PlatformTypeReadListDTO>>()
                .ForMember(dest => dest.Entities, opt => opt.MapFrom(src => src));

            CreateMap<PlatformTypeCreateDTO, PlatformType>()
                .ForMember(dest => dest.GamePlatformTypes, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<PlatformTypeUpdateDTO, PlatformType>()
                 .ForMember(dest => dest.GamePlatformTypes, opt => opt.Ignore())
                 .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<Game, GameReadDTO>()
                .ForMember(dest => dest.PlatformTypes, opt => opt.MapFrom(src => src.GamePlatformTypes))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.GameGenres))
                .ForMember(dest => dest.PublisherReadDTO, opt => opt.MapFrom(src => src.Publisher));
            
            CreateMap<Game, GameReadListDTO>();

            CreateMap<PagedList<Game>, PagedListDTO<GameReadListDTO>>()
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
                .ForMember(dest => dest.Games, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<Publisher, PublisherReadDTO>()
                .ForMember(dest => dest.GameReadListDTOs, opt => opt.MapFrom(src => src.Games));

            CreateMap<Publisher, PublisherReadListDTO>();

            CreateMap<PagedList<Publisher>, PagedListDTO<PublisherReadListDTO>>()
                .ForMember(dest => dest.Entities, opt => opt.MapFrom(src => src));

            CreateMap<OrderCreateDTO, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.ListOfOrderDetails, opt => opt.Ignore())
                .ForMember(dest => dest.IsPaid, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.ShippedDate, opt => opt.Ignore());

            CreateMap<Order, OrderReadListDTO>()
                .ForMember(dest => dest.CustomerNickName, opt => opt.MapFrom(src => src.Customer.NickName));

            CreateMap<Order, OrderReadDTO>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.ListOfOrderDetails))
                .ForMember(dest => dest.CustomerNickName, opt => opt.MapFrom(src => src.Customer.NickName));

            CreateMap<OrderDetail, OrderDetailsReadListDTO>()
                .ForMember(dest => dest.GameKey, opt => opt.MapFrom(src => src.Game.Key))
                .ForMember(dest => dest.OrderDetailsId, opt => opt.Ignore());

            CreateMap<UserCreateDTO, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.UserRole, opt => opt.Ignore())
                .ForMember(dest => dest.RoleId, opt => opt.Ignore())
                .ForMember(dest => dest.Publisher, opt => opt.Ignore())
                .ForMember(dest => dest.PublisherId, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshTokenId, opt => opt.Ignore())
                .ForMember(dest => dest.BannedTo, opt => opt.Ignore())
                .ForMember(dest => dest.Orders, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());

            CreateMap<User, UserReadListDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.UserRole.Name))
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<PagedList<User>, PagedListDTO<UserReadListDTO>>()
                .ForMember(dest => dest.Entities, opt => opt.MapFrom(src => src));

            CreateMap<Role, RoleReadListDTO>();

            CreateMap<PagedList<Role>, PagedListDTO<RoleReadListDTO>>()
                .ForMember(dest => dest.Entities, opt => opt.MapFrom(src => src));

            CreateMap<RoleCreateDTO, Role>()
                .ForMember(dest => dest.UsersRole, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
