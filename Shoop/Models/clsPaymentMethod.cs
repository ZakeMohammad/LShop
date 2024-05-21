using System.ComponentModel.DataAnnotations;

namespace Shoop.Models
{
	public class clsPaymentMethod
	{
		[Key]
		public int ID { get; set; }

		public string PaymentName { get; set;}
	}
}
