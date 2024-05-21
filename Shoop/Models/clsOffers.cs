using System.ComponentModel.DataAnnotations;

namespace Shoop.Models
{
    public class clsOffers
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; } = null!;
        public string Detils { get; set; } = null!;
        public string Image { get; set; } = null!;
        public int GategoreID { get; set; }

        public int OfferRatio { get; set; }


    }
}
