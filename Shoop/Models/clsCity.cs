using System.ComponentModel.DataAnnotations;

namespace Shoop.Models
{
	public class clsCity
	{
		[Key]
		public int ID { get; set; }

		public string CityName { get; set; }
		public decimal ShippingAmount { get; set; }
	}
}
