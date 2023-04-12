using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.Exceptions
{
    public class NotFoundException : Exception
    {
        private const string DefaultMessage = "Not found";
        public NotFoundException():base(DefaultMessage)
        {
        }
        public NotFoundException(string message) : base(message)
        {
        }
        public NotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
