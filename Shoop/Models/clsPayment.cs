using System.ComponentModel.DataAnnotations;

namespace Shoop.Models
{
    public class clsPayment
    {
        [Key]
        public int ID { get; set; }

        public int UserID { get; set; }

        public int OrderID { get; set; }

        public DateTime Date { get; set; }
        public decimal OrderAmount { get; set; }

		public int PaymentMethod { get; set; }
		public string ShippingAdrees { get; set; }
		public int City { get; set; }
		
		public string PhoneNumber { get; set; }
		public string Email { get; set; }


	}
}
