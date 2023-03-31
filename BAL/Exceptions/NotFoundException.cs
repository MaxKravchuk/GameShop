using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Exceptions
{
    public class NotFoundException : Exception
    {
        public new string Message { get; set; } = string.Empty;

        public NotFoundException()
        {
            Message = "We coudn't find that";
        }
        public NotFoundException(string message) : base(message)
        {
            Message = message;
        }
        public NotFoundException(string message, Exception inner) : base(message, inner)
        {
            Message = message;
        }
    }
}
