using System.ComponentModel.DataAnnotations;

namespace Shoop.Models
{
    public class clsContactMessages
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; } = null!;


        public string Email { get; set; } = null!;


        public string Message { get; set; } = null!;


        public string Subject { get; set; } = null!;
    }
}
