using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shoop.Models
{
    public class clsProduct
    {
        [Key]
        [JsonProperty]
        public int ID { get; set; }
        [JsonProperty]
        public string Name { get; set; } = null!;
        [JsonProperty]
        public string Description { get; set; }
        [JsonProperty]
        public string ImageOne { get; set; } = null!;
        [JsonProperty]
        public string ImageTwo { get; set; }
        [JsonProperty]
        public string ImageThere { get; set; }
        [JsonProperty]
        public string ImageFour { get; set; }
        [JsonProperty]
        public string ImageFive { get; set; }
        [JsonProperty]
        public decimal Price { get; set; }
        [JsonProperty]
        public int CategoryID { get; set; }
        [JsonProperty]
        public int Quantity { get; set; }
        [JsonProperty]
        public int Rating { get; set; }


    }

}
