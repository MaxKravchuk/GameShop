using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.Exceptions
{
    public class BadRequestException : Exception
    {
        private const string DefaultMessage = "Illegal request";
        public BadRequestException() : base(DefaultMessage)
        {
        }
        public BadRequestException(string message) : base(message)
        {
        }
        public BadRequestException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
