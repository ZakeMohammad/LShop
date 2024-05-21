using System.ComponentModel.DataAnnotations;

namespace Shoop.Models
{
    public class clsShoppingCart
    {
        [Key]
        public int ID { get; set; }
        public int? UserID { get; set; }
        public int? ProductID { get; set; }
        public int Quantity { get; set; }
     

    }
}
