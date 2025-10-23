using CarStore.DAL;
using Microsoft.EntityFrameworkCore;
using CarStore.DAL.Repositories;
using CarStore.BLL.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;
using CarStore.WebUI.Hubs;
using CarStore.WebUI.Services;

var builder = WebApplication.CreateBuilder(args);

// Set culture to Vietnamese for currency formatting
var cultureInfo = new CultureInfo("vi-VN");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Add services to the container.
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizeAreaFolder("Admin", "/", "Admin");
    });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";
        options.LogoutPath = "/Users/Logout";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
});

builder.Services.AddDbContext<CarStoreDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CarStoreDB")));

// Register Repositories
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ITestDriveRepository, TestDriveRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register Services
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ITestDriveService, TestDriveService>();
builder.Services.AddScoped<IUserService, UserService>();

// Register SignalR Notification Service
builder.Services.AddScoped<ISignalRNotificationService, SignalRNotificationService>();

// Add SignalR
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Map SignalR Hubs
app.MapHub<CarStoreHub>("/hubs/carstore");
app.MapHub<OrderHub>("/hubs/orders");
app.MapHub<TestDriveHub>("/hubs/testdrives");
app.MapHub<InventoryHub>("/hubs/inventory");
app.MapHub<AdminHub>("/hubs/admin");

app.Run();
