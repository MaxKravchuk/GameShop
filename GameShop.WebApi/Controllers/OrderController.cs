using System.Threading.Tasks;
using System.Web.Http;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Filters;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/orders")]
    [JwtAuthorize]
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

        [HttpGet]
        [Route("getAll")]
        public async Task<IHttpActionResult> GetAllAsync()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Json(orders);
        }

        [HttpGet]
        [Route("getById/{orderId}")]
        public async Task<IHttpActionResult> GetByIdAsync(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            return Json(order);
        }

        [HttpPut]
        [Route("updateStatus")]
        public async Task<IHttpActionResult> UpdateOrderStatusAsync([FromBody] OrderUpdateDTO orderUpdateDTO)
        {
            await _orderService.UpdateOrderStatusAsync(orderUpdateDTO);
            return Ok();
        }

        [HttpPut]
        [Route("updateOrderDetails")]
        public async Task<IHttpActionResult> UpdateOrderDetailsAsync([FromBody] OrderUpdateDTO orderUpdateDTO)
        {
            await _orderService.UpdateOrderDetailsAsync(orderUpdateDTO);
            return Ok();
        }
    }
}
