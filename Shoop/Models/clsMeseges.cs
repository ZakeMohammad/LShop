using System.ComponentModel.DataAnnotations;

namespace Shoop.Models
{
	public class clsMeseges
	{
		[Key]
		public int ID { get; set; }

		public int UserID { get; set; }

		public string Message { get; set; }

		public bool IsShow { get; set; }

		public DateTime Date { get; set; }
	}
}
