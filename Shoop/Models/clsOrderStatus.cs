using System.ComponentModel.DataAnnotations;

namespace Shoop.Models
{
	public class clsOrderStatus
	{
		[Key]
		public int ID { get; set; }

		public string Status { get; set; }
	}
}
