// <copyright file="GameServiceTest.cs">Copyright ©  2023</copyright>
using System;
using System.Threading.Tasks;
using GameShop.BLL.DTO.FilterDTOs;
using GameShop.BLL.DTO.GameDTOs;
using GameShop.BLL.DTO.PaginationDTOs;
using GameShop.BLL.Services;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameShop.BLL.Services.Tests
{
    /// <summary>This class contains parameterized unit tests for GameService</summary>
    [PexClass(typeof(GameService))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class GameServiceTest
    {
        /// <summary>Test stub for GetAllGamesAsync(GameFiltersDTO)</summary>
        [PexMethod]
        public Task<PagedListViewModel<GameReadListDTO>> GetAllGamesAsyncTest(
            [PexAssumeUnderTest]GameService target,
            GameFiltersDTO gameFiltersDTO
        )
        {
            Task<PagedListViewModel<GameReadListDTO>> result
               = target.GetAllGamesAsync(gameFiltersDTO);
            return result;
            // TODO: add assertions to method GameServiceTest.GetAllGamesAsyncTest(GameService, GameFiltersDTO)
        }
    }
}
