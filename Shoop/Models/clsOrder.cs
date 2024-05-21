using System.ComponentModel.DataAnnotations;

namespace Shoop.Models
{
    public class clsOrder
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; } 
     



     
    }
}
