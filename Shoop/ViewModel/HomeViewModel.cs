using Shoop.Models;

namespace Shoop.ViewModel
{
    public class HomeViewModel
    {
        public List<clsOffers> AllOffers { get; set; } = new List<clsOffers>();

        public List<dynamic> TrandyProduct { get; set; } = new List<dynamic>();

        public List<dynamic> ArrivedProduct { get; set; } = new List<dynamic>();

        public List<clsGategore> Gategores { get; set; } = null!;

        public List<dynamic> AllCommints { get; set; } = new List<dynamic>();

    }
}
