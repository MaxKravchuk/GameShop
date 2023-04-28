using System;
using AutoMapper;
using GameShop.BLL.Services.Utils;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces;
using GameShop.DAL.Context;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository;
using GameShop.DAL.Repository.Interfaces;
using log4net;
using Unity;
using Unity.Lifetime;
using GameShop.BLL.Services.Interfaces.Utils;
using FluentValidation;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.Services.Utils.Validators;
using GameShop.BLL.DTO.CommentDTOs;
using GameShop.BLL.DTO.PublisherDTOs;
using StackExchange.Redis;
using System.Configuration;
using GameShop.DAL.Repository.Interfaces.Utils;
using GameShop.BLL.DTO.RedisDTOs;

namespace GameShop.WebApi.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<GameShopContext>(new HierarchicalLifetimeManager());

            container.RegisterType<IRepository<Comment>, Repository<Comment>>();
            container.RegisterType<IRepository<Game>, Repository<Game>>();
            container.RegisterType<IRepository<Genre>, Repository<Genre>>();
            container.RegisterType<IRepository<PlatformType>, Repository<PlatformType>>();
            container.RegisterType<IRepository<Publisher>, Repository<Publisher>>();

            container.RegisterType<IUnitOfWork, UnitOfWork>();

            var redisConnectionString = ConfigurationManager.ConnectionStrings["RedisConnectingString"].ConnectionString;
            var redisConfiguration = ConfigurationOptions.Parse(redisConnectionString);
            var redis = ConnectionMultiplexer.Connect(redisConnectionString);
            container.RegisterInstance(redis);
            container.RegisterType<IRedisProvider<CartItemDTO>, RedisProvider<CartItemDTO>>();

            container.RegisterType<ICommentService, CommentService>();
            container.RegisterType<IGameService, GameService>();
            container.RegisterType<IGenreService, GenreService>();
            container.RegisterType<IPlatformTypeService, PlatformTypeService>();
            container.RegisterType<IPublisherService, PublisherService>();
            container.RegisterType<IShoppingCartService, ShoppingCartService>();

            var log = LogManager.GetLogger(typeof(LoggerManager));
            container.RegisterInstance(typeof(ILog), log);
            container.RegisterType<ILoggerManager, LoggerManager>();

            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
            });
            container.RegisterInstance<IMapper>(mapperConfiguration.CreateMapper());

            container.RegisterType<IValidatorFactory, UnityValidatorFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<IValidator<GameCreateDTO>, GameCreateDTOValidator>
                (new ContainerControlledLifetimeManager());
            container.RegisterType<IValidator<CommentCreateDTO>, CommentCreateDTOValidator>
                (new ContainerControlledLifetimeManager());
            container.RegisterType<IValidator<PublisherCreateDTO>, PublisherCreateDTOValidator>
                (new ContainerControlledLifetimeManager());
            container.RegisterType<IValidator<CartItemDTO>, CartItemValidator>
                (new ContainerControlledLifetimeManager());
        }
    }
}
