using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApplication4.DataLayer.DbContext;
using WebApplication4.DataLayer.Initializers;
using Microsoft.AspNetCore.Authorization;
using WebApplication4.Entities;
using WebApplication4.BusinessLogic.Services;
using WebApplication4.BusinessLogic.Factories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<UpdatedUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAuthenticatedUser", policy => policy.RequireAuthenticatedUser());
    options.AddPolicy("AdminAccess", policy => policy.RequireClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", "Admin"));
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o => o.IdleTimeout = TimeSpan.FromMinutes(2));

// Register Business Logic Layer Services
builder.Services.AddScoped<IOrderService>(provider => new OrderService(connectionString));
builder.Services.AddScoped<IProductService>(provider => new ProductService(connectionString));
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICategoryService>(provider => new CategoryService(connectionString));
builder.Services.AddScoped<RepositoryFactory>(provider => new RepositoryFactory(connectionString));
var app = builder.Build();

// Initialize database tables (Product, Category, Order, OrderProducts)
DatabaseInitializer.InitializeDatabase(connectionString);

// Seed sample data for testing (only if database is empty)
SampleDataSeeder.SeedSampleData(connectionString);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseSession();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();


app.Run();
