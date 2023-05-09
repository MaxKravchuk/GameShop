using System.Collections.Generic;

namespace GameShop.BLL.DTO.PlatformTypeDTOs
{
    public class PlatformTypeUpdateDTO
    {
        public string Type { get; set; }

        public IEnumerable<string> GameKeys { get; set; } = new List<string>();
    }
}
