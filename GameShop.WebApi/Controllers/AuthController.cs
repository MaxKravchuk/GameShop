using System.Threading.Tasks;
using System.Web.Http;
using GameShop.BLL.DTO.UserDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Services.Interfaces.Utils;
using GameShop.WebApi.Filters;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/auth")]
    public class AuthController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IUsersTokenService _usersTokenService;

        public AuthController(
            IUserService userService,
            IJwtTokenProvider jwtTokenProvider,
            IUsersTokenService usersTokenService)
        {
            _userService = userService;
            _jwtTokenProvider = jwtTokenProvider;
            _usersTokenService = usersTokenService;
        }

        [HttpGet]
        [Route("login")]
        public async Task<IHttpActionResult> LoginAsync([FromUri] UserCreateDTO userCreateDTO)
        {
            var isValid = await _userService.IsValidUserCredentialsAsync(userCreateDTO);
            if (!isValid)
            {
                throw new BadRequestException("Invalid credentials");
            }

            var role = await _userService.GetRoleAsync(userCreateDTO.NickName);
            var authResponse = _jwtTokenProvider.GetAuthenticatedResponse(userCreateDTO.NickName, role);
            await _usersTokenService.AddUserTokenAsync(userCreateDTO.NickName, authResponse.RefreshToken);

            return Json(authResponse);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IHttpActionResult> RegisterAsync([FromBody] UserCreateDTO userCreateDTO)
        {
            await _userService.CreateUserAsync(userCreateDTO);
            return Ok();
        }
    }
}
