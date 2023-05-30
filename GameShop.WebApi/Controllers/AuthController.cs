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

        public AuthController(
            IUserService userService,
            IJwtTokenProvider jwtTokenProvider)
        {
            _userService = userService;
            _jwtTokenProvider = jwtTokenProvider;
        }

        [HttpGet]
        [Route("login")]
        [JwtAuthenticationFilter]
        public async Task<IHttpActionResult> LoginAsync([FromUri] UserCreateDTO userCreateDTO)
        {
            var isValid = await _userService.IsValidUserCredentials(userCreateDTO);
            if (!isValid)
            {
                throw new BadRequestException("Invalid credentials");
            }

            var role = await _userService.GetRoleAsync(userCreateDTO.NickName);
            var token = _jwtTokenProvider.GenerateToken(userCreateDTO.NickName, role);
            return Ok(token);
        }

        [HttpPost]
        [Route("register")]
        [JwtAuthenticationFilter]
        public async Task<IHttpActionResult> RegisterAsync(UserCreateDTO userCreateDTO)
        {
            await _userService.CreateUser(userCreateDTO);
            return Ok();
        }
    }
}
