using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using GameShop.BLL.Strategies;
using GameShop.BLL.Strategies.Interfaces;

namespace GameShop.WebApi.Controllers
{
    [RoutePrefix("api/payments")]
    public class PaymentController : ApiController
    {
        private readonly IPaymentContext _paymentContext;

        public PaymentController(
            IPaymentContext paymentContext)
        {
            _paymentContext = paymentContext;
        }
    }
}
