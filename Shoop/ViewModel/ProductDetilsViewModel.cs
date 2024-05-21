using Microsoft.EntityFrameworkCore;
using Shoop.Data.Services;
using Shoop.Models;

namespace Shoop.ViewModel
{
    public class ProductDetilsViewModel
    {
      
        private readonly ReviewsService _reviwService;
        public ProductDetilsViewModel( ReviewsService reviwService)
        {
           ;
            _reviwService = reviwService;

        }

        public clsProduct Product { get; set; } = null!;


        public List<clsProduct> clsProducts { get; set; } = null!;

        public List<dynamic> AllReviews { get; set; } = null!;


        public clsReviews Review { get; set; } 


    }
}
