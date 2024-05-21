using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Shoop.Models;
using System;

namespace Shoop.Data
{
    public class ApplicationDBContext:DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<clsAdmin> Admins { get; set; }
        public DbSet<clsOrder> Orders { get; set; }
        public DbSet<clsOrderDetails> OrderDetails { get; set; }
        public DbSet<clsProduct> Products { get; set; }
        public DbSet<clsUser> Users { get; set; }
		public DbSet<clsPayment> Payments { get; set; }
		public DbSet<clsShoppingCart> ShoppingCarts { get; set; }

        public DbSet<clsGategore> Gategoryes { get; set; }
        public DbSet<clsOffers> Offers { get; set; }
        public DbSet<clsFavorite> Favorites { get; set; }

        public DbSet<clsReviews> Reviews { get; set; }

		public DbSet<clsOrderStatus> OrderStatus { get; set; }

		public DbSet<clsCity> Citys { get; set; }
		public DbSet<clsPaymentMethod> PaymentMethods { get; set; }

		public DbSet<clsMeseges> Messages { get; set; }

        public DbSet<clsContactMessages> ContactMessages {  get; set; }
		public DbSet<clsTrandyAndArrivedProducts> TrandyAndArrivedProducts { get; set; }
		

	}
}
