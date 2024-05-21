using System.ComponentModel.DataAnnotations;

namespace Shoop.Models
{
    public class clsReviews
    {

        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ProductID { get; set; }

        public string Review { get; set; } = null!;
        public DateTime Date { get; set; }

        public int Rating { get; set; }

        public bool IsShow { get; set;}

    }
}
