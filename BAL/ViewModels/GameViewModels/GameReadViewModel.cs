using BAL.ViewModels.ComentViewModels;
using BAL.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ViewModels.GameViewModels
{
    public class GameReadViewModel : GameBaseViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public IEnumerable<GameGenreViewModel> Genres { get; set; }
        public IEnumerable<GamePlatformTypeViewModel> PlatformTypes { get; set; }
        public IEnumerable<ComentReadViewModel> Coments { get; set; }

    }
}
