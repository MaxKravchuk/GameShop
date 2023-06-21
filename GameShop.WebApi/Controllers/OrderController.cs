using System.Threading.Tasks;
using System.Web.Http;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Filters;

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
        [JwtAuthorize]
        public async Task<IHttpActionResult> CreateOrderAsync([FromBody] OrderCreateDTO orderCreateDTO)
        {
            var newOrderId = await _orderService.CreateOrderAsync(orderCreateDTO);

            return Ok(newOrderId);
        }

        [HttpGet]
        [Route("getAll")]
        [JwtAuthorize(Roles = "Manager")]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Json(orders);
        }

        [HttpGet]
        [Route("getById/{orderId}")]
        [JwtAuthorize(Roles = "Manager")]
        public async Task<IHttpActionResult> GetByIdAsync(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            return Json(order);
        }

        [HttpPut]
        [Route("updateStatus")]
        [JwtAuthorize(Roles = "Manager")]
        public async Task<IHttpActionResult> UpdateOrderStatusAsync([FromBody] OrderUpdateDTO orderUpdateDTO)
        {
            await _orderService.UpdateOrderStatusAsync(orderUpdateDTO);
            return Ok();
        }

        [HttpPut]
        [Route("updateOrderDetails")]
        [JwtAuthorize(Roles = "Manager")]
        public async Task<IHttpActionResult> UpdateOrderDetailsAsync([FromBody] OrderUpdateDTO orderUpdateDTO)
        {
            await _orderService.UpdateOrderDetailsAsync(orderUpdateDTO);
            return Ok();
        }
    }
}
