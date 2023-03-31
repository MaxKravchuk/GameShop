using BAL.ViewModels.GameViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ViewModels.PlatformTypeViewModels
{
    public class PlatformTypeReadViewModel
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public IEnumerable<GameReadListViewModel> Games { get; set; } = new List<GameReadListViewModel>();
    }
}
