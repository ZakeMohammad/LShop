using Shoop.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace Shoop.Models
{
    public class clsUser
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ImageURL { get; set; } = null!;
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

        public int Purchasesnumber { get; set; }
    }
}
