using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameShop.BLL.DTO.PlatformTypeDTOs
{
    public class PlatformTypeUpdateDTO
    {
        public string Type { get; set; }
        public IEnumerable<string> GameKeys { get; set; } = new List<string>();
    }
}
