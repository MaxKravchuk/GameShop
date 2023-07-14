using System.Configuration;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using FluentValidation;
using GameShop.BLL.DTO.CommentDTOs;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.DTO.PlatformTypeDTOs;
using GameShop.BLL.DTO.PublisherDTOs;
using GameShop.BLL.DTO.RedisDTOs;
using GameShop.BLL.DTO.RoleDTOs;
using GameShop.BLL.DTO.UserDTOs;
using GameShop.BLL.Enums;
using GameShop.BLL.Filters;
using GameShop.BLL.Filters.Interfaces;
using GameShop.BLL.Services;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.BLL.Services.Utils;
using GameShop.BLL.Services.Utils.Validators;
using GameShop.BLL.Strategies.BanStrategies;
using GameShop.BLL.Strategies.Factories;
using GameShop.BLL.Strategies.Interfaces.Factories;
using GameShop.BLL.Strategies.Interfaces.Strategies;
using GameShop.BLL.Strategies.PaymentStrategies;
using GameShop.BLL.Strategies.SortingStrategies;
using GameShop.DAL.Context;
using GameShop.DAL.Entities;
using GameShop.DAL.Repository;
using GameShop.DAL.Repository.Interfaces;
using GameShop.DAL.Repository.Interfaces.Utils;
using GameShop.DAL.Repository.Utils;
using GameShop.WebApi.App_Start;
using log4net;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
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

            container.RegisterType<ICommentRepository,CommentRepository>();
            container.RegisterType<IRepository<Game>, Repository<Game>>();
            container.RegisterType<IRepository<Genre>, Repository<Genre>>();
            container.RegisterType<IRepository<PlatformType>, Repository<PlatformType>>();
            container.RegisterType<IRepository<Publisher>, Repository<Publisher>>();
            container.RegisterType<IRepository<DAL.Entities.Order>, Repository<DAL.Entities.Order>>();
            container.RegisterType<IRepository<OrderDetail>, Repository<OrderDetail>>();
            container.RegisterType<IRepository<User>, Repository<User>>();
            container.RegisterType<IRepository<UserTokens>, Repository<UserTokens>>();
            container.RegisterType<IRepository<DAL.Entities.Role>, Repository<DAL.Entities.Role>>();

            container.RegisterType<IUnitOfWork, UnitOfWork>();

            var redisConnectionString = ConfigurationManager.ConnectionStrings["RedisConnectingString"].ConnectionString;
            var redisConfiguration = ConfigurationOptions.Parse(redisConnectionString);
            var redis = ConnectionMultiplexer.Connect(redisConnectionString);
            container.RegisterInstance(redis);
            container.RegisterType<IRedisProvider<CartItemDTO>, RedisProvider<CartItemDTO>>(new HierarchicalLifetimeManager());
            
            // The working azure service bus should exists to work with this code
            /*var serviceBusConnectionString = ConfigurationManager.ConnectionStrings["AzureServiceBusConnectionString"].ConnectionString;
            var serviceBusQueueName = ConfigurationManager.ConnectionStrings["AzureServiceBusQueueName"].ConnectionString;
            var azureSBClient = new QueueClient(serviceBusConnectionString, serviceBusQueueName);
            container.RegisterInstance(azureSBClient);
            container.RegisterType<IServiceBusProvider, ServiceBusProvider>(new HierarchicalLifetimeManager());*/

            var blobContainerConnectionString = ConfigurationManager.ConnectionStrings["AzureBlobConnectionString"].ConnectionString;
            var storageAccount = CloudStorageAccount.Parse(blobContainerConnectionString);
            var blobServiceClient = storageAccount.CreateCloudBlobClient();
            container.RegisterInstance(blobServiceClient);
            container.RegisterType<IBlobStorageProvider, BlobStorageProvider>(new HierarchicalLifetimeManager());

            container.RegisterType<ICommentService, CommentService>();
            container.RegisterType<IGameService, GameService>();
            container.RegisterType<IGenreService, GenreService>();
            container.RegisterType<IPlatformTypeService, PlatformTypeService>();
            container.RegisterType<IPublisherService, PublisherService>();
            container.RegisterType<IShoppingCartService, ShoppingCartService>();
            container.RegisterType<IOrderService, OrderService>();
            container.RegisterType<IPaymentService, PaymentService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IUsersTokenService, UsersTokenService>();
            container.RegisterType<IRoleService, RoleService>();
            container.RegisterType<IPasswordProvider, PasswordProvider>();
            container.RegisterType<IJwtTokenProvider, JwtTokenProvider>();

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
            container.RegisterType<IValidator<UserCreateDTO>, UserCreateDtoValidator>
                (new ContainerControlledLifetimeManager());
            container.RegisterType<IValidator<GenreCreateDTO>, GenreCreateDtoValidator>
                (new ContainerControlledLifetimeManager());
            container.RegisterType<IValidator<PlatformTypeCreateDTO>, PlatformTypeCreateDtoValidator>
                (new ContainerControlledLifetimeManager());
            container.RegisterType<IValidator<RoleCreateDTO>, RoleCreateDtoValidator>
                (new ContainerControlledLifetimeManager());

            container.RegisterType<IPaymentStrategyFactory, PaymentStrategyFactory>();
            container.RegisterType<IPaymentStrategy, BankStrategy>(PaymentTypes.Bank.ToString());
            container.RegisterType<IPaymentStrategy, IBoxStrategy>(PaymentTypes.IBox.ToString());
            container.RegisterType<IPaymentStrategy, VisaStrategy>(PaymentTypes.Visa.ToString());

            container.RegisterType<IFiltersFactory<IQueryable<Game>>, GameFiltersFactory>();
            container.RegisterType<IOperation<IQueryable<Game>>, CreatedAtFilter>();
            container.RegisterType<IOperation<IQueryable<Game>>, GenreFilter>();
            container.RegisterType<IOperation<IQueryable<Game>>, NameFilter>();
            container.RegisterType<IOperation<IQueryable<Game>>, PlatformTypeFilter>();
            container.RegisterType<IOperation<IQueryable<Game>>, PriceFilter>();
            container.RegisterType<IOperation<IQueryable<Game>>, PublisherFilter>();

            container.RegisterType<IGameSortingFactory, SortingStrategyFactory>();
            container.RegisterType<IGamesSortingStrategy, AscPriceStrategy>();
            container.RegisterType<IGamesSortingStrategy, DateStrategy>();
            container.RegisterType<IGamesSortingStrategy, DescPriceStrategy>();
            container.RegisterType<IGamesSortingStrategy, MostCommentedStrategy>();
            container.RegisterType<IGamesSortingStrategy, MostPopularStrategy>();

            container.RegisterType<IBanFactory, BanFactory>();
            container.RegisterType<IBanStrategy, HourBanStrategy>();
            container.RegisterType<IBanStrategy, DayBanStrategy>();
            container.RegisterType<IBanStrategy, MonthBanStrategy>();
            container.RegisterType<IBanStrategy, PermanentBanStrategy>();
            container.RegisterType<IBanStrategy, WeekBanStrategy>();


            httpConfiguration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}
