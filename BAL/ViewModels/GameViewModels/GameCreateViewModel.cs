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
        public IEnumerable<string> GenresName { get; set; } = new List<string>();
        public IEnumerable<string> PlatformTypeName { get; set; } = new List<string>();
    }
}
