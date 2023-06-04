using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using GameShop.BLL.DTO.PaymentDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.WebApi.Filters;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/payments")]
    [JwtAuthorize]
    public class PaymentController : ApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly IShoppingCartService _shoppingCartService;

        public PaymentController(
            IPaymentService paymentService,
            IShoppingCartService shoppingCartService)
        {
            _paymentService = paymentService;
            _shoppingCartService = shoppingCartService;
        }

        [HttpPost]
        [Route("payAndGetInvoice")]
        public async Task<IHttpActionResult> GetInvoiceAsync([FromBody] PaymentCreateDTO paymentCreateDTO)
        {
            var paymentResult = await _paymentService.ExecutePaymentAsync(paymentCreateDTO);
            await _shoppingCartService.CleatCartAsync();

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

        [HttpPost]
        [Route("pay")]
        public async Task<IHttpActionResult> PayAsync([FromBody] PaymentCreateDTO paymentCreateDTO)
        {
            var paymentResult = await _paymentService.ExecutePaymentAsync(paymentCreateDTO);
            await _shoppingCartService.CleatCartAsync();
            return Json(paymentResult.OrderId);
        }
    }
}
