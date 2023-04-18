using System;
using AutoMapper;
using Xunit;
using GameShop.WebApi.App_Start;
using GameShop.BLL.DTO.CommentDTOs;
using GameShop.DAL.Entities;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.DTO.GenreDTOs;
using GameShop.BLL.DTO.PlatformTypeDTOs;
using System.Runtime.Serialization;

namespace BLL.Test
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(config =>
                config.AddProfile<AutoMapperConfiguration>());
            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Theory]
        [InlineData(typeof(CommentCreateDTO), typeof(Comment))]
        [InlineData(typeof(Comment), typeof(CommentReadDTO))]
        [InlineData(typeof(GameCreateDTO), typeof(Game))]
        [InlineData(typeof(Genre), typeof(GenreReadListDTO))]
        [InlineData(typeof(PlatformType), typeof(PlatformTypeReadListDTO))]
        [InlineData(typeof(Game), typeof(GameReadDTO))]
        [InlineData(typeof(Game), typeof(GameReadListDTO))]
        [InlineData(typeof(GameUpdateDTO), typeof(Game))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = GetInstanceOf(source);

            _mapper.Map(instance, source, destination);
        }

        private object GetInstanceOf(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(type);

            return FormatterServices.GetUninitializedObject(type);
        }
    }
}
