using Microsoft.AspNetCore.Localization;
using OnlineShopWebApp;
using System.Globalization;
using Serilog;
using OnlineShop.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using OnlineShop.Db.Models;
using OnlineShopWebApp.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string connection = builder.Configuration.GetConnectionString("online_shop");
builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));

// добавляем контекст IndentityContext в качестве сервиса в приложение
builder.Services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connection));
// указываем тип пользователя и роли
builder.Services.AddIdentity<User, IdentityRole>()
				// устанавливаем тип хранилища - наш контекст
				.AddEntityFrameworkStores<IdentityContext>();

// настройка cookie
builder.Services.ConfigureApplicationCookie(options =>
{
	options.ExpireTimeSpan = TimeSpan.FromHours(8);
	options.LoginPath = "/Account/Login";
	options.LogoutPath = "/Account/Logout";
	options.Cookie = new CookieBuilder
	{
		IsEssential = true
	};
});

builder.Services.AddTransient<IProductsRepository, ProductsDbRepository>(); // 1
builder.Services.AddTransient<ICartsRepository, CartsDbRepository>(); // 2
builder.Services.AddTransient<IOrdersRepository, OrdersDbRepository>(); // 3
builder.Services.AddTransient<IFavoriteRepository, FavoriteDbRepository>(); // 4
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

builder.Services.AddAutoMapper(typeof(MappingProfile));


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

// подключение аутентификации
app.UseAuthentication();

// подключение авторизации
app.UseAuthorization();

// инициализация администратора
using (var serviceScope = app.Services.CreateScope())
{
	var services = serviceScope.ServiceProvider;
	var userManager = services.GetRequiredService<UserManager<User>>();
	var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
	IdentityInitializer.Initialize(userManager, rolesManager);
}

app.UseSerilogRequestLogging();

app.UseRequestLocalization();

app.MapControllerRoute(
	name: "MyArea",
	pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
