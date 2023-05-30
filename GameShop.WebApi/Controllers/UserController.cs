using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using GameShop.BLL.Services.Interfaces;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


    }
}
