using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ViewModels.PlatformTypeViewModels
{
    public class PlatformTypeUpdateViewModel
    {
        public string Type { get; set; }
        public IEnumerable<string> GameKeys { get; set; } = new List<string>();
    }
}
