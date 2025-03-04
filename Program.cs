using Microsoft.AspNetCore.Localization;
using OnlineShopWebApp;
using System.Globalization;
using Serilog;
using OnlineShop.Db;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connection = builder.Configuration.GetConnectionString("online_shop");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));
builder.Services.AddTransient<IProductsRepository, ProductsDbRepository>(); // 1
builder.Services.AddTransient<ICartsRepository, CartsDbRepository>(); // 2
builder.Services.AddTransient<IOrdersRepository, OrdersDbRepository>(); // 3
builder.Services.AddTransient<IFavoriteRepository, FavoriteDbRepository>(); // 4
builder.Services.AddSingleton<IRolesRepository, InMemoryRolesRepository>(); // 5
builder.Services.AddSingleton<IUsersManager, UsersManager>(); // 6
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	var supportedCultures = new[]
	{
		new CultureInfo("en-US")
	};
	options.DefaultRequestCulture = new RequestCulture("en-US");
	options.SupportedCultures = supportedCultures;
	options.SupportedUICultures = supportedCultures;
});
builder.Host.UseSerilog((context, configuration) => configuration
.ReadFrom.Configuration(context.Configuration)
.Enrich.WithProperty("ApplicationName", "Online Shop"));
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.UseRequestLocalization();

app.MapControllerRoute(
	name: "MyArea",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
