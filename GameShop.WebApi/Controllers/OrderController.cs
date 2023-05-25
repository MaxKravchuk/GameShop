using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.Services.Interfaces;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/orders")]
    public class OrderController : ApiController
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Route()]
        public async Task<IHttpActionResult> CreateOrderAsync([FromBody] OrderCreateDTO orderCreateDTO)
        {
            var newOrderId = await _orderService.CreateOrderAsync(orderCreateDTO);

            return Ok(newOrderId);
        }
    }
}
