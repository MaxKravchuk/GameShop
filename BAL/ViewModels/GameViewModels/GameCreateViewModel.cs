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
        public IEnumerable<int> GenresName { get; set; } = new List<int>();
        public IEnumerable<int> PlatformTypeName { get; set; } = new List<int>();
    }
}
