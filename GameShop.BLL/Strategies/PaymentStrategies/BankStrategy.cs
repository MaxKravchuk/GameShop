using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameShop.BLL.DTO.OrderDTOs;
using GameShop.BLL.DTO.StrategyDTOs;
using GameShop.BLL.Services.Interfaces;
using GameShop.BLL.Strategies.Interfaces;
using GameShop.BLL.Strategies.Interfaces.Strategies;
using GameShop.DAL.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GameShop.BLL.Strategies.PaymentStrategies
{
    public class BankStrategy : IPaymentStrategy
    {
        public PaymentResultDTO Pay(Order newOrder)
        {
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

            var result = new PaymentResultDTO
            {
                InvoiceMemoryStream = new MemoryStream(Encoding.ASCII.GetBytes(invoice.ToString())),
                OrderId = newOrder.Id
            };

            return result;
        }
    }
}
