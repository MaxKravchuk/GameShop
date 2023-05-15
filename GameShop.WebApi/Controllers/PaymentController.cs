using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.Exceptions;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Strategies;
using GameShop.BLL.Strategies.Interfaces;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/payments")]
    public class PaymentController : ApiController
    {
        private readonly IOrderService _orderService;
        private readonly IShoppingCartService _shoppingCartService;

        public PaymentController(
            IOrderService orderService,
            IShoppingCartService shoppingCartService)
        {
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost]
        [Route("pay")]
        public async Task<IHttpActionResult> PayAsync([FromBody] OrderCreateDTO orderCreateDTO)
        {
            var paymentResult = await _orderService.ExecutePayment(orderCreateDTO);
            await _shoppingCartService.CleatCartAsync();

            if (paymentResult.InvoiceMemoryStream != null)
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new StreamContent(paymentResult.InvoiceMemoryStream);
                result.Content.Headers.ContentLength = paymentResult.InvoiceMemoryStream.Length;
                result.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition =
                    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                result.Content.Headers.ContentDisposition.FileName = $"{paymentResult.OrderId}.txt";

                return ResponseMessage(result);
            }
            else
            {
                return Json(paymentResult.OrderId);
            }
        }
    }
}
