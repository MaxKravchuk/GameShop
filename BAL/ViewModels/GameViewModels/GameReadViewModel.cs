using BAL.ViewModels.ComentViewModels;
using BAL.ViewModels.GenreViewModels;
using BAL.ViewModels.Helpers;
using BAL.ViewModels.PlatformTypeViewModels;
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
        public IEnumerable<GenreReadListViewModel> Genres { get; set; }
        public IEnumerable<PlatformTypeReadListViewModel> PlatformTypes { get; set; }

    }
}
