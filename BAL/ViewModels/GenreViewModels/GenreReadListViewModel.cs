using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.ViewModels.GenreViewModels
{
    public class GenreReadListViewModel : GenreBaseViewModel
    {
        public int Id { get; set; }
        public IEnumerable<string> SubGenresName { get; set; } = new List<string>();
    }
}
