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
        public IEnumerable<GamePropsViewModel> Genres { get; set; } = new List<GamePropsViewModel>();
        public IEnumerable<GamePropsViewModel> PlatformTypes { get; set; } = new List<GamePropsViewModel>();
        public IEnumerable<ComentReadViewModel> Coments { get; set; } = new List<ComentReadViewModel>();

    }
}
