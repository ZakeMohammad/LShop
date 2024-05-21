using System.ComponentModel.DataAnnotations;

namespace Shoop.Models
{
	public class clsTrandyAndArrivedProducts
	{
		[Key]
		public int ID { get; set; }

		public int ProductID { get; set; }


		public bool IsTrandy { get; set; }
	}
}
