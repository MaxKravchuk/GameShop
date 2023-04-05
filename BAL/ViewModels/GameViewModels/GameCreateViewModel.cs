using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ViewModels.GameViewModels
{
    public class GameCreateViewModel : GameBaseViewModel
    {
        public string Description { get; set; }
        public IEnumerable<int> GenresId { get; set; } = new List<int>();
        public IEnumerable<int> PlatformTypeId { get; set; } = new List<int>();
    }
}
