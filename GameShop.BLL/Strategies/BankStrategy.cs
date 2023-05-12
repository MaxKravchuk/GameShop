using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Strategies.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GameShop.BLL.Strategies
{
    public class BankStrategy : IPaymentStrategy<MemoryStream>
    {
        public async Task<MemoryStream> Pay(OrderCreateDTO orderCreateDTO, IOrderService orderService)
        {
            var newOrder = await orderService.CreateOrderAsync(orderCreateDTO);

            var invoice = new StringBuilder();

            invoice.Append($"Customer number: {newOrder.CustomerId}");
            invoice.Append($"Ordered at: {newOrder.OrderedAt}");
            invoice.Append("With this list of games:");

            var gameList = new StringBuilder();

            foreach (var orderDetail in newOrder.ListOfOrderDetails)
            {
                var totalPrice =
                    orderDetail.Quantity * (decimal)orderDetail.Game.Price * (decimal)(1 - orderDetail.Discount);

                gameList.Append($"Game name: {orderDetail.Game.Name}\n");
                gameList.Append($"Quantity: {orderDetail.Quantity}\n");
                gameList.Append($"Total price: {totalPrice}\n");
                gameList.Append($"Discount: {orderDetail.Discount}\n\n");
            }

            var formattedGameList = gameList.ToString();
            invoice.Append(formattedGameList);

            return new MemoryStream(Encoding.ASCII.GetBytes(invoice.ToString()));
        }
    }
}
