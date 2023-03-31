using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ViewModels.GenreViewModels
{
    public class GenreReadViewModel : GenreBaseViewModel
    {
        public int Id { get; set; }
        public IEnumerable<string> GameKeys { get; set; } = new List<string>();
    }
}
