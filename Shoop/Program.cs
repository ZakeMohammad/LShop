using Microsoft.EntityFrameworkCore;
using Shoop.Data;
using Shoop.Data.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnectionString")));
builder.Services.AddControllersWithViews();
builder.Services.AddResponseCompression(options =>
{
	options.EnableForHttps = true;
});

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IUserSerevice, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderDetailsService, OrderDetailsService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IGategoreService, GategoreService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<GategoreService>();
builder.Services.AddScoped<OffersService>();
builder.Services.AddScoped<FavoriteService>();
builder.Services.AddScoped<ReviewsService>();
builder.Services.AddScoped<ShoppingCartService>();
builder.Services.AddScoped<OrderDetailsService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<ContactMessagesService>();
builder.Services.AddScoped<TrandyAndArrivedProductsService>();

// Add services to the container.


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
	app.UseDeveloperExceptionPage();
}
app.UseResponseCompression();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
