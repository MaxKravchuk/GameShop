using System.Configuration;
using System.IO;
using System.Web.Http;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.CommentDTOs;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.DTO.PublisherDTOs;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.BLL.Services.Utils;
using GameShop.BLL.Services.Utils.Validators;
using GameShop.BLL.Strategies;
using GameShop.BLL.Strategies.Interfaces;
using GameShop.DAL.Context;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository;
using GameShop.DAL.Repository.Interfaces;
using GameShop.DAL.Repository.Interfaces.Utils;
using GameShop.WebApi.App_Start;
using log4net;
using StackExchange.Redis;
using Unity;
using Unity.Lifetime;
using Unity.WebApi;

namespace GameShop.WebApi
{
    public static class UnityConfig
    {
        public static void RegisterComponents(HttpConfiguration httpConfiguration)
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<GameShopContext>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<Comment>, Repository<Comment>>();
            container.RegisterType<IRepository<Game>, Repository<Game>>();
            container.RegisterType<IRepository<Genre>, Repository<Genre>>();
            container.RegisterType<IRepository<PlatformType>, Repository<PlatformType>>();
            container.RegisterType<IRepository<Publisher>, Repository<Publisher>>();
            container.RegisterType<IRepository<DAL.Entities.Order>, Repository<DAL.Entities.Order>>();
            container.RegisterType<IRepository<OrderDetails>, Repository<OrderDetails>>();

            container.RegisterType<IUnitOfWork, UnitOfWork>();

            var redisConnectionString = ConfigurationManager.ConnectionStrings["RedisConnectingString"].ConnectionString;
            var redisConfiguration = ConfigurationOptions.Parse(redisConnectionString);
            var redis = ConnectionMultiplexer.Connect(redisConnectionString);
            container.RegisterInstance(redis);
            container.RegisterType<IRedisProvider<CartItemDTO>, RedisProvider<CartItemDTO>>(new HierarchicalLifetimeManager());

            container.RegisterType<ICommentService, CommentService>();
            container.RegisterType<IGameService, GameService>();
            container.RegisterType<IGenreService, GenreService>();
            container.RegisterType<IPlatformTypeService, PlatformTypeService>();
            container.RegisterType<IPublisherService, PublisherService>();
            container.RegisterType<IShoppingCartService, ShoppingCartService>();
            container.RegisterType<IOrderService, OrderService>();

            var log = LogManager.GetLogger(typeof(LoggerManager));
            container.RegisterInstance(typeof(ILog), log);
            container.RegisterType<ILoggerManager, LoggerManager>();

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
            });
            container.RegisterInstance<IMapper>(mapperConfiguration.CreateMapper());

            container.RegisterType<IValidatorFactory, UnityValidatorFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<IValidator<GameCreateDTO>, GameCreateDtoValidator>
                (new ContainerControlledLifetimeManager());
            container.RegisterType<IValidator<CommentCreateDTO>, CommentCreateDtoValidator>
                (new ContainerControlledLifetimeManager());
            container.RegisterType<IValidator<PublisherCreateDTO>, PublisherCreateDtoValidator>
                (new ContainerControlledLifetimeManager());
            container.RegisterType<IValidator<CartItemDTO>, CartItemValidator>
                (new ContainerControlledLifetimeManager());
            container.RegisterType<IValidator<OrderCreateDTO>, OrderCreateDtoValidator>
                (new ContainerControlledLifetimeManager());

            container.RegisterType<IPaymentContext<MemoryStream>,PaymentContext<MemoryStream>>();
            container.RegisterType<IPaymentContext<int>,PaymentContext<int>>();
            container.RegisterType<IPaymentContext<int>,PaymentContext<int>>();
            container.RegisterType<IPaymentStrategy<MemoryStream>, BankStrategy>();
            container.RegisterType<IPaymentStrategy<int>, IBoxStrategy>();
            container.RegisterType<IPaymentStrategy<int>, VisaStrategy>();

            httpConfiguration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
