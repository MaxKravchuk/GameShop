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
        private readonly IPaymentContext<MemoryStream> _paymentBankContext;
        private readonly IPaymentContext<int> _paymentiBoxContext;
        private readonly IPaymentContext<int> _paymentVisaContext;
        private readonly IOrderService _orderService;
        private readonly IShoppingCartService _shoppingCartService;

        public PaymentController(
            IPaymentContext<MemoryStream> paymentBankContext,
            IPaymentContext<int> paymentiBoxContext,
            IPaymentContext<int> paymentVisaContext,
            IOrderService orderService,
            IShoppingCartService shoppingCartService)
        {
            _paymentBankContext = paymentBankContext;
            _paymentiBoxContext = paymentiBoxContext;
            _paymentVisaContext = paymentVisaContext;
            _orderService = orderService;
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost]
        [Route("bank")]
        public async Task<HttpResponseMessage> PayBankAsync([FromBody] OrderCreateDTO orderCreateDTO)
        {
            var orderStream = await _paymentBankContext.ExecuteStrategy(orderCreateDTO, _orderService);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(orderStream);
            result.Content.Headers.ContentLength = orderStream.Length;
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = $"{orderCreateDTO.CustomerID}.txt";

            await _shoppingCartService.CleatCartAsync();

            return result;
        }

        [HttpPost]
        [Route("iBox")]
        public async Task<int> PayiBoxAsync([FromBody] OrderCreateDTO orderCreateDTO)
        {
            var orderInt = await _paymentiBoxContext.ExecuteStrategy(orderCreateDTO, _orderService);

            await _shoppingCartService.CleatCartAsync();

            return orderInt;
        }

        [HttpPost]
        [Route("visa")]
        public async Task<int> PayVisaAsync([FromBody] OrderCreateDTO orderCreateDTO)
        {
            var orderInt = await _paymentVisaContext.ExecuteStrategy(orderCreateDTO, _orderService);

            await _shoppingCartService.CleatCartAsync();

            return orderInt;
        }
    }
}
